using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScripts.Game
{
    public class FieldViewModel
    {
        public ShapeViewModelsContainer shapeViewModelsContainer;
        
        private FieldModel _fieldModel;
        private IShapeCatalog _shapeCatalog;
        private RectInt _rect;

        public FieldViewModel(FieldModel fieldModel, IShapeCatalog shapeCatalog)
        {
            _fieldModel = fieldModel;
            _shapeCatalog = shapeCatalog;
            _rect = new RectInt(0, 0, 9, 9);
            shapeViewModelsContainer = new ShapeViewModelsContainer();
        }

        public bool CanPlaceShape(int uid, Rotation rotation, Vector2Int cell)
        {
            var shapeData = _shapeCatalog.Shapes[uid];
            foreach (var point in shapeData.PointsAfterRotation(rotation))
            {
                var pointPositionOnGrid = cell + point;
                if (!_rect.Contains(pointPositionOnGrid))
                    return false;
                if (_fieldModel.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].uid != 0)
                    return false;
            }
            return true;
        }

        public void PlaceShape(int uid, Rotation rotation, Vector2Int cell)
        {
            if (!CanPlaceShape(uid, rotation, cell))
                return;
            var shapeData = _shapeCatalog.Shapes[uid];
            foreach (var point in shapeData.PointsAfterRotation(rotation))
            {
                var pointPositionOnGrid = cell + point;
                _fieldModel.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].uid = shapeData.Uid;
                _fieldModel.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].shapeRotation = rotation;
                _fieldModel.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].positionInShape = point;
            }
        }

        public HashSet<Vector2Int> FindCellsToDelete()
        {
            HashSet<Vector2Int> cellsToDelete = new HashSet<Vector2Int>();
            var completedRows = Enumerable.Range(0, 9).ToList();
            var completedColumns = Enumerable.Range(0, 9).ToList();
            var completedSubgrids = Enumerable.Range(0, 9).ToList();

            for (int column = 0; column < 9; column++)
            {
                for (int row = 0; row < 9; row++)
                {
                    if (_fieldModel.FieldMatrix[column, row].uid == 0)
                    {
                        completedColumns.Remove(column);
                        completedRows.Remove(row);
                        completedSubgrids.Remove(GetSubgridId(row, column));
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

        public HashSet<Vector2Int> FindBrokenShapesCells()
        {
            var visitedCells = new HashSet<Vector2Int>();
            var brokenShapesCells = new HashSet<Vector2Int>();
            var potentiallyBrokenParts = new List<Vector2Int>();
            
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    var cell = _fieldModel.FieldMatrix[x, y];
                    if(cell.uid == 0 || visitedCells.Contains(new Vector2Int(x, y))) 
                        continue;

                    var shapeData = _shapeCatalog.Shapes[cell.uid];
                    var shapeOrigin = new Vector2Int(x, y) - cell.positionInShape;
                    potentiallyBrokenParts.Clear();
                    bool shapeIsBroken = false;
                    foreach (var positionInShape in shapeData.PointsAfterRotation(cell.shapeRotation))
                    {
                        var pointPositionOnGrid = shapeOrigin + positionInShape;
                        if (_fieldModel.FieldMatrix[pointPositionOnGrid].uid == shapeData.Uid)
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

        public void DestroyCells(HashSet<Vector2Int> cells)
        {
            foreach (var cell in cells)
            {
                DestroyCell(cell);
            }
        }

        private void DestroyCell(Vector2Int cell)
        {
            if (_fieldModel.FieldMatrix[cell.x, cell.y].hp > 1)
            {
                _fieldModel.FieldMatrix[cell.x, cell.y].hp -= 1;
                return;
            }
            _fieldModel.FieldMatrix[cell.x, cell.y].uid = 0;
            _fieldModel.FieldMatrix[cell.x, cell.y].hp = 0;
        }

        private int GetSubgridId(int x, int y)
        {
            return x / 3 + y / 3 * 3;
        }

        private int GetSubgridId(Vector2Int cell)
        {
            return GetSubgridId(cell.x, cell.y);
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

        private HashSet<Vector2Int> AllCellsInRow(int row)
        {
            var cells = new HashSet<Vector2Int>();
            for (int i = 0; i < 9; i++)
            {
                cells.Add(new Vector2Int(i, row));
            }
            return cells;
        }
        
        private HashSet<Vector2Int> AllCellsInSubgrid(int subgridId)
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

        public bool PreviewShapePlacement(int uid, Rotation rotation, Vector2Int cell, out List<Vector2Int> occupiedCells)
        {
            occupiedCells = new List<Vector2Int>();
            if (!CanPlaceShape(uid, rotation, cell)) 
                return false;

            var shapeData = _shapeCatalog.Shapes[uid];
            foreach (var point in shapeData.PointsAfterRotation(rotation))
            {
                var pointPositionOnGrid = cell + point;
                occupiedCells.Add(pointPositionOnGrid);
            }
            return true;
        }
    }
}