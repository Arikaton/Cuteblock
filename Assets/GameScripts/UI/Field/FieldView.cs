using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.Game
{
    public class FieldView : MonoBehaviour
    {
        [SerializeField] private CellView cellViewPrefab;
        [SerializeField] private RectTransform cellContainerRect;
        [SerializeField] private GridLayoutGroup gridLayout;
        [SerializeField] private ActiveShapeContainer shapeContainer;

        private CellView[,] _cellViews;

        private void Awake()
        {
            _cellViews = new CellView[9, 9];
        }

        [Button, DisableInEditorMode]
        public void SetupCells()
        {
            gridLayout.cellSize = new Vector2(cellContainerRect.rect.width / 9, cellContainerRect.rect.height / 9);
            gridLayout.startCorner = GridLayoutGroup.Corner.LowerLeft;
            gridLayout.startAxis = GridLayoutGroup.Axis.Vertical;
            gridLayout.childAlignment = TextAnchor.LowerLeft;
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            gridLayout.constraintCount = 9;
            
            for (var x = 0; x < 9; x++)
            {
                for (var y = 0; y < 9; y++)
                {
                    InstantiateCellViewAt(x, y);
                }
            }
        }

        private void InstantiateCellViewAt(int x, int y)
        {
            var cellView = Instantiate(cellViewPrefab, cellContainerRect);
            _cellViews[x, y] = cellView;
        }
    }
}