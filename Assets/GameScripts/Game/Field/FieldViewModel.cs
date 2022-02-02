using System;
using System.Collections.Generic;
using System.Linq;
using GameScripts.ConsumeSystem.Module;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameScripts.Game
{
    public class FieldViewModel
    {
        public IReadOnlyReactiveCollection<ShapeViewModel> ShapesOnField;
        public IReadOnlyReactiveCollection<ShapeViewModel> AvailableShapes;
        public IReadOnlyReactiveProperty<int> Score;
        public IReadOnlyReactiveProperty<bool> HighlightShapesOnField;
        public IReadOnlyReactiveProperty<bool> HighlightAvailableShapes;
        public CellViewModel[,] CellViewModels;
        public ReactiveCommand OnGameFinished;

        private IReactiveCollection<ShapeViewModel> _shapesOnField;
        private IReactiveCollection<ShapeViewModel> _availableShapes;
        private IReactiveProperty<int> _score;
        public IReactiveProperty<bool> _highlightShapesOnField;
        public IReactiveProperty<bool> _highlightAvailableShapes;
        private FieldModel _model;
        private IShapeCatalog _shapeCatalog;
        private AbstractConsumableFactory _consumableFactory;
        private List<Vector2Int> _shadowedCells;
        private List<Vector2Int> _highlightedCells;
        private RectInt _rect = new RectInt(0, 0, 9, 9);

        public FieldViewModel(FieldModel model, IShapeCatalog shapeCatalog, AbstractConsumableFactory consumableFactory)
        {
            _model = model;
            _shapeCatalog = shapeCatalog;
            _consumableFactory = consumableFactory;
            OnGameFinished = new ReactiveCommand();
            _shapesOnField = new ReactiveCollection<ShapeViewModel>();
            _availableShapes = new ReactiveCollection<ShapeViewModel>(new List<ShapeViewModel>(3) {null, null, null});
            _score = new ReactiveProperty<int>(0);
            _highlightShapesOnField = new ReactiveProperty<bool>(false);
            _highlightAvailableShapes = new ReactiveProperty<bool>(false);
            ShapesOnField = _shapesOnField;
            AvailableShapes = _availableShapes;
            Score = _score;
            HighlightShapesOnField = _highlightShapesOnField;
            HighlightAvailableShapes = _highlightAvailableShapes;
            CellViewModels = new CellViewModel[9, 9];
            _shadowedCells = new List<Vector2Int>();
            _highlightedCells = new List<Vector2Int>();

            Initialize();
        }

        private void Initialize()
        {
            for (var x = 0; x < 9; x++)
            {
                for (var y = 0; y < 9; y++)
                {
                    CellViewModels[x, y] = new CellViewModel(_model.FieldMatrix[x, y]);
                }
            }
            
            for (int i = 0; i < _model.AvailableShapes.Length; i++)
            {
                var shapeModel = _model.AvailableShapes[i];
                if (shapeModel == null) continue;
                var shapeViewModel = new ShapeViewModel(shapeModel, _shapeCatalog.Shapes[shapeModel.Uid], this);
                _availableShapes[i] = shapeViewModel;
            }
            
            // TODO: Предварительно найти клетки с количеством hp
            // TODO: Предварительно найти все сломанные фигуры и заменить на целые
            var shapeModelsOnField = FindAllShapeModelsOnField();
            foreach (var foundShape in shapeModelsOnField)
            {
                var shapeModel = foundShape.model;
                var shapeViewModel = new ShapeViewModel(shapeModel, _shapeCatalog.Shapes[shapeModel.Uid], this);
                shapeViewModel.PlaceShapeAt(foundShape.origin);
                _shapesOnField.Add(shapeViewModel);
            }
            CheckRemainingShapesAvailability();
        }

        public void UseRotateHint()
        {
            _highlightAvailableShapes.Value = true;
            foreach (var shapeViewModel in _availableShapes)
            {
                shapeViewModel?.EnableHighlighting();
            }
        }

        public void UseReplacementHint()
        {
            for (int i = 0; i < 3; i++)
            {
                _availableShapes[i]?.Destroy.Execute();
                _availableShapes[i] = null;
                _model.AvailableShapes[i] = null;
                
                var newShapeId = Random.Range(1, 11);
                var newShapeRotation = ExtensionMethods.GetRandomRotation();
                var shapeModel = new ShapeModel(newShapeId, newShapeRotation); 
                var shapeViewModel = new ShapeViewModel(shapeModel, _shapeCatalog.Shapes[newShapeId], this);
                _availableShapes[i] = shapeViewModel;
                _model.AvailableShapes[i] = shapeModel;
            }
            CheckRemainingShapesAvailability();
        }
        
        public void UseDeleteHint()
        {
            _highlightShapesOnField.Value = true;
            foreach (var shapeViewModel in _shapesOnField)
            {
                shapeViewModel.EnableHighlighting();
            }
        }

        public void ClickShape(ShapeViewModel shapeViewModel)
        {
            if (_highlightAvailableShapes.Value)
            {
                _highlightAvailableShapes.Value = false;
                foreach (var shape in _availableShapes)
                {
                    shape?.DisableHighlighting();
                }
                shapeViewModel.RotateClockwise();
                CheckRemainingShapesAvailability();
                return;
            }

            if (_highlightShapesOnField.Value)
            {
                _highlightShapesOnField.Value = false;
                foreach (var shape in _shapesOnField)
                {
                    shape?.DisableHighlighting();
                }
                var cellsToDelete = FindCellsOfShapeViewModel(shapeViewModel);
                ClearCells(cellsToDelete);
                shapeViewModel.Destroy.Execute();
                _shapesOnField.Remove(shapeViewModel);
                CheckRemainingShapesAvailability();
            }
        }

        private HashSet<Vector2Int> FindCellsOfShapeViewModel(ShapeViewModel shape)
        {
            if (shape.PositionOnGrid.Value == new Vector2Int(-1, -1))
                throw new InvalidOperationException("Cannot find cells of shape which is not placed on field");
            var output = new HashSet<Vector2Int>();
            var originOnField = shape.PositionOnGrid.Value;
            var rotation = shape.Rotation.Value;
            foreach (var point in shape.ShapeData.PointsAfterRotation(rotation))
            {
                var pointOnField = originOnField + point;
                output.Add(pointOnField);
                if (_model.FieldMatrix[pointOnField].uid.Value != shape.Uid)
                    throw new InvalidOperationException("Cannot find cell of shape which has already been broken");
            }
            return output;
        }

        public bool PlaceShape(int shapeIndex, Vector2Int cell)
        {
            if (cell == new Vector2Int(-1, 1)) return false;
            var shapeViewModel = _availableShapes[shapeIndex];
            if (!CanPlaceShape(shapeViewModel.Uid, shapeViewModel.Rotation.Value, cell))
                return false;
            var shapeData = _shapeCatalog.Shapes[shapeViewModel.Uid];
            foreach (var point in shapeData.PointsAfterRotation(shapeViewModel.Rotation.Value))
            {
                var pointPositionOnGrid = cell + point;
                _model.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].uid.Value = shapeViewModel.Uid;
                _model.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].shapeRotation = shapeViewModel.Rotation.Value;
                _model.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].positionInShape = point;
            }
            
            PlaceShapeViewModel(shapeIndex, cell);
            CancelAllShadowing();
            CancelAllHighlighting();
            var cellsToDelete = FindCellsToDelete();
            AddScore(cellsToDelete.Count * 5);
            var shapesToDestroy = FindShapesToDestroy(cellsToDelete);
            ClearCells(cellsToDelete);
            DestroyShapes(shapesToDestroy);
            var brokenCells = FindBrokenShapesCells();
            FillBrokenCellsAdvanced(brokenCells);
            CreateNewAvailableShapes(shapeIndex);
            CheckRemainingShapesAvailability();
            return true;
        }

        public void PreviewShapePlacement(int shapeIndex, Vector2Int cell)
        {
            if (cell == new Vector2Int(-1, -1))
            {
                CancelAllShadowing();
                CancelAllHighlighting();
                return;
            }
            var shapeViewModel = _availableShapes[shapeIndex];
            var occupiedCells = new HashSet<Vector2Int>();
            if (!CanPlaceShape(shapeViewModel.Uid, shapeViewModel.Rotation.Value, cell))
            {
                CancelAllShadowing();
                CancelAllHighlighting();
                return;
            }

            var shapeData = _shapeCatalog.Shapes[shapeViewModel.Uid];
            foreach (var point in shapeData.PointsAfterRotation(shapeViewModel.Rotation.Value))
            {
                var pointPositionOnGrid = cell + point;
                occupiedCells.Add(pointPositionOnGrid);
            }
            ShadowCells(occupiedCells);
            var highlightedCells = FindCellsToDeleteAfterShapePlacement(occupiedCells);
            HighlightCells(highlightedCells);
        }

        private bool CanPlaceShape(int uid, Rotation rotation, Vector2Int cell)
        {
            var shapeData = _shapeCatalog.Shapes[uid];
            foreach (var point in shapeData.PointsAfterRotation(rotation))
            {
                var pointPositionOnGrid = cell + point;
                if (!_rect.Contains(pointPositionOnGrid))
                    return false;
                if (_model.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].uid.Value != 0)
                    return false;
            }
            return true;
        }

        private void AddScore(int count)
        {
            _score.Value += count;
            _model.Score = _score.Value;
        }

        private void FillBrokenCells(HashSet<Vector2Int> brokenCells)
        {
            var newShapeId = 6;
            var newShapeRotation = Rotation.Deg0;
            
            foreach (var cell in brokenCells)
            {
                var shapeData = _shapeCatalog.Shapes[newShapeId];
                foreach (var point in shapeData.PointsAfterRotation(newShapeRotation))
                {
                    var pointPositionOnGrid = cell + point;
                    _model.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].uid.Value = newShapeId;
                    _model.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].shapeRotation = newShapeRotation;
                    _model.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].positionInShape = point;
                }
                
                var shapeModel = new ShapeModel(newShapeId, newShapeRotation);
                var shapeViewModel = new ShapeViewModel(shapeModel, _shapeCatalog.Shapes[newShapeId], this);
                shapeViewModel.PlaceShapeAt(cell);
                shapeViewModel.CanBePlaced.Value = true;
                _shapesOnField.Add(shapeViewModel);
            }
        }

        private void FillBrokenCellsAdvanced(HashSet<Vector2Int> brokenCells)
        {
            var regions = FieldTools.FillPointsWithShapes(brokenCells, _shapeCatalog.AllShapes);
            foreach (var region in regions)
            {
                var shapeData = _shapeCatalog.Shapes[region.shapeId];
                foreach (var point in shapeData.PointsAfterRotation(region.shapeRotation))
                {
                    var pointPositionOnGrid = region.origin + point;
                    _model.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].uid.Value = region.shapeId;
                    _model.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].shapeRotation = region.shapeRotation;
                    _model.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].positionInShape = point;
                }
                var shapeModel = new ShapeModel(region.shapeId, region.shapeRotation);
                var shapeViewModel = new ShapeViewModel(shapeModel, _shapeCatalog.Shapes[region.shapeId], this);
                shapeViewModel.PlaceShapeAt(region.origin);
                shapeViewModel.CanBePlaced.Value = true;
                _shapesOnField.Add(shapeViewModel);
            }
        }

        private void DestroyShapes(List<ShapeViewModel> shapesToDestroy)
        {
            foreach (var shape in shapesToDestroy)
            {
                shape.Destroy.Execute();
                _shapesOnField.Remove(shape);
            }
        }

        private List<ShapeViewModel> FindShapesToDestroy(HashSet<Vector2Int> cellsToDelete)
        {
            var foundShapes = new List<ShapeViewModel>();
            var visitedCells = new HashSet<Vector2Int>();

            foreach (var cellPosition in cellsToDelete)
            {
                var cell = _model.FieldMatrix[cellPosition];
                if (visitedCells.Contains(cellPosition))
                    continue;
                var shapeData = _shapeCatalog.Shapes[cell.uid.Value];
                var shapeOrigin = cellPosition - cell.positionInShape;
                foreach (var shapePoint in shapeData.PointsAfterRotation(cell.shapeRotation))
                    visitedCells.Add(shapeOrigin + shapePoint);
                var shapeOnField = _shapesOnField.First(x => x.PositionOnGrid.Value == shapeOrigin);
                if (shapeOnField == null)
                    throw new InvalidOperationException("Field contains occupied cells which do not belong to any shapes");
                foundShapes.Add(shapeOnField);
            }
            return foundShapes;
        }

        private void CheckRemainingShapesAvailability()
        {
            bool gameFinished = true;
            foreach (var shape in _availableShapes)
            {
                if (shape == null) continue;
                var shapeCanBePlaced = ShapeCanBePlaced(shape.Uid, shape.Rotation.Value);
                shape.CanBePlaced.SetValueAndForceNotify(shapeCanBePlaced);
                if (shapeCanBePlaced)
                    gameFinished = false;
            }
            if (gameFinished)
                OnGameFinished.Execute();
        }

        private void PlaceShapeViewModel(int shapeIndex, Vector2Int cell)
        {
            var shapeViewModel = _availableShapes[shapeIndex];
            shapeViewModel.PlaceShapeAt(cell);
            _shapesOnField.Insert(_shapesOnField.Count, null);
            _shapesOnField[_shapesOnField.Count - 1] = shapeViewModel;
        }

        private void CreateNewAvailableShapes(int shapeIndex)
        {
            _availableShapes[shapeIndex] = null;
            _model.AvailableShapes[shapeIndex] = null;
            if (_availableShapes[0] != null || _availableShapes[1] != null || _availableShapes[2] != null)
                return;

            for (int i = 0; i < 3; i++)
            {
                var newShapeId = Random.Range(1, 11);
                var newShapeRotation = ExtensionMethods.GetRandomRotation();
                var shapeModel = new ShapeModel(newShapeId, newShapeRotation); 
                var shapeViewModel = new ShapeViewModel(shapeModel, _shapeCatalog.Shapes[newShapeId], this);
                _availableShapes[i] = shapeViewModel;
                _model.AvailableShapes[i] = shapeModel;
            }
        }

        private HashSet<Vector2Int> FindCellsToDelete()
        {
            HashSet<Vector2Int> cellsToDelete = new HashSet<Vector2Int>();
            var completedRows = Enumerable.Range(0, 9).ToList();
            var completedColumns = Enumerable.Range(0, 9).ToList();
            var completedSubgrids = Enumerable.Range(0, 9).ToList();

            for (int column = 0; column < 9; column++)
            {
                for (int row = 0; row < 9; row++)
                {
                    if (_model.FieldMatrix[column, row].uid.Value == 0)
                    {
                        completedColumns.Remove(column);
                        completedRows.Remove(row);
                        completedSubgrids.Remove(GetSubgridId(column, row));
                    }
                }
            }

            foreach (var row in completedRows)
                cellsToDelete.UnionWith(AllCellsInRow(row));
            foreach (var column in completedColumns) 
                cellsToDelete.UnionWith(AllCellsInColumn(column));
            foreach (var subgrid in completedSubgrids)
                cellsToDelete.UnionWith(AllCellsInSubgrid(subgrid));

            return cellsToDelete;
        }

        private HashSet<Vector2Int> FindBrokenShapesCells()
        {
            var visitedCells = new HashSet<Vector2Int>();
            var brokenShapesCells = new HashSet<Vector2Int>();
            var potentiallyBrokenParts = new List<Vector2Int>();
            
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    var cell = _model.FieldMatrix[x, y];
                    if(cell.uid.Value == 0 || visitedCells.Contains(new Vector2Int(x, y))) 
                        continue;

                    var shapeData = _shapeCatalog.Shapes[cell.uid.Value];
                    var shapeOrigin = new Vector2Int(x, y) - cell.positionInShape;
                    potentiallyBrokenParts.Clear();
                    bool shapeIsBroken = false;
                    foreach (var positionInShape in shapeData.PointsAfterRotation(cell.shapeRotation))
                    {
                        var pointPositionOnGrid = shapeOrigin + positionInShape;
                        if (_model.FieldMatrix[pointPositionOnGrid].uid.Value == shapeData.Uid)
                            potentiallyBrokenParts.Add(pointPositionOnGrid);
                        else
                            shapeIsBroken = true;
                    }
                    visitedCells.UnionWith(potentiallyBrokenParts);
                    if (shapeIsBroken) brokenShapesCells.UnionWith(potentiallyBrokenParts);
                }
            }
            return brokenShapesCells;
        }

        private List<(Vector2Int origin, ShapeModel model)> FindAllShapeModelsOnField()
        {
            var shapeModels = new List<(Vector2Int origin, ShapeModel model)>();
            var visitedCells = new HashSet<Vector2Int>();
            
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    var cell = _model.FieldMatrix[x, y];
                    if(cell.uid.Value == 0 || visitedCells.Contains(new Vector2Int(x, y))) 
                        continue;
                    
                    var shapeData = _shapeCatalog.Shapes[cell.uid.Value];
                    var shapeOrigin = new Vector2Int(x, y) - cell.positionInShape;
                    var shapeModel = new ShapeModel(cell.uid.Value, cell.shapeRotation);
                    shapeModels.Add((shapeOrigin, shapeModel));
                    
                    foreach (var positionInShape in shapeData.PointsAfterRotation(cell.shapeRotation))
                    {
                        var pointPositionOnGrid = shapeOrigin + positionInShape;
                        visitedCells.Add(pointPositionOnGrid);
                        if (_model.FieldMatrix[pointPositionOnGrid].uid.Value != shapeData.Uid)
                            throw new InvalidOperationException("Cannot find shapes models on field with broken cells");
                    }
                }
            }
            return shapeModels;
        }

        private void ClearCells(HashSet<Vector2Int> cells)
        {
            foreach (var cell in cells)
            {
                ClearCell(cell);
            }
        }

        private void ClearCell(Vector2Int cell)
        {
            if (_model.FieldMatrix[cell.x, cell.y].hp.Value > 1)
            {
                _model.FieldMatrix[cell.x, cell.y].hp.Value -= 1;
                return;
            }
            _model.FieldMatrix[cell.x, cell.y].uid.Value = 0;
            _model.FieldMatrix[cell.x, cell.y].hp.Value = 0;
        }

        private int GetSubgridId(int x, int y)
        {
            return x / 3 + y / 3 * 3;
        }

        private HashSet<Vector2Int> AllCellsInColumn(int column)
        {
            var cells = new HashSet<Vector2Int>();
            for (int i = 0; i < 9; i++)
            {
                cells.Add(new Vector2Int(column, i));
            }
            return cells;
        }

        private static HashSet<Vector2Int> AllCellsInRow(int row)
        {
            var cells = new HashSet<Vector2Int>();
            for (int i = 0; i < 9; i++)
            {
                cells.Add(new Vector2Int(i, row));
            }
            return cells;
        }

        private static HashSet<Vector2Int> AllCellsInSubgrid(int subgridId)
        {
            var x0 = subgridId % 3 * 3;
            var y0 = subgridId / 3 * 3;
            var cells = new HashSet<Vector2Int>();
            for (int xOffset = 0; xOffset < 3; xOffset++)
            {
                for (int yOffset = 0; yOffset < 3; yOffset++)
                {
                    cells.Add(new Vector2Int(x0 + xOffset, y0 + yOffset));
                }
            }
            return cells;
        }

        private bool ShapeCanBePlaced(int uid, Rotation rotation)
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (CanPlaceShape(uid, rotation, new Vector2Int(x, y)))
                        return true;
                }
            }
            return false;
        }

        private void ShadowCells(HashSet<Vector2Int> shadowedCells)
        {
            CancelAllShadowing();
            foreach (var cell in shadowedCells)
            {
                _shadowedCells.Add(cell);
                CellViewModels[cell.x, cell.y].TurnOnShadow();
            }
        }

        private void HighlightCells(HashSet<Vector2Int> highlightedCells)
        {
            CancelAllHighlighting();
            foreach (var cell in highlightedCells)
            {
                _highlightedCells.Add(cell);
                CellViewModels[cell.x, cell.y].TurnOnHighlight();
            }
        }

        private void CancelAllShadowing()
        {
            foreach (var cell in _shadowedCells)
            {
                CellViewModels[cell.x, cell.y].TurnOffShadow();
            }
            _shadowedCells.Clear();
        }

        private void CancelAllHighlighting()
        {
            foreach (var cell in _highlightedCells)
            {
                CellViewModels[cell.x, cell.y].TurnOffHighlight();
            }
        }

        private HashSet<Vector2Int> FindCellsToDeleteAfterShapePlacement(HashSet<Vector2Int> occupiedPoints)
        {
            HashSet<Vector2Int> cellsToDelete = new HashSet<Vector2Int>();
            var completedRows = Enumerable.Range(0, 9).ToList();
            var completedColumns = Enumerable.Range(0, 9).ToList();
            var completedSubgrids = Enumerable.Range(0, 9).ToList();

            for (int column = 0; column < 9; column++)
            {
                for (int row = 0; row < 9; row++)
                {
                    if (_model.FieldMatrix[column, row].uid.Value == 0 && !occupiedPoints.Contains(new Vector2Int(column, row)))
                    {
                        completedColumns.Remove(column);
                        completedRows.Remove(row);
                        completedSubgrids.Remove(GetSubgridId(column, row));
                    }
                }
            }

            foreach (var row in completedRows)
                cellsToDelete.UnionWith(AllCellsInRow(row));
            foreach (var column in completedColumns) 
                cellsToDelete.UnionWith(AllCellsInColumn(column));
            foreach (var subgrid in completedSubgrids)
                cellsToDelete.UnionWith(AllCellsInSubgrid(subgrid));

            return cellsToDelete;
        }
    }
}