using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScripts.Game
{
    public class FieldViewModel
    {
        private FieldModel _fieldModel;
        private RectInt _rect;

        public FieldViewModel(FieldModel fieldModel)
        {
            _fieldModel = fieldModel;
            _rect = new RectInt(0, 0, 8, 8);
        }

        public bool CanPlaceShape(ShapeViewModel shape, Vector2Int cell)
        {
            foreach (var point in shape.PointsAfterRotation())
            {
                var pointPositionOnGrid = cell + point;
                if (!_rect.Contains(pointPositionOnGrid))
                    return false;
                if (_fieldModel.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].uid != 0)
                    return false;
            }
            return true;
        }

        public void PlaceShape(ShapeViewModel shape, Vector2Int cell)
        {
            if (!CanPlaceShape(shape, cell))
                return;
            foreach (var point in shape.PointsAfterRotation())
            {
                var pointPositionOnGrid = cell + point;
                _fieldModel.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].uid = shape.Uid;
                _fieldModel.FieldMatrix[pointPositionOnGrid.x, pointPositionOnGrid.y].shapeRotation = shape.Rotation;
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
        
        private int GetSubgridId(int x, int y)
        {
            return x / 3 + y / 3 * 3;
        }

        private int GetSubgridId(Vector2Int cell)
        {
            return cell.x / 3 + cell.y / 3 * 3;
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
    }
}