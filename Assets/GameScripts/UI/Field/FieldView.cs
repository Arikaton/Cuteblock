using System.Collections.Generic;
using GameScripts.Game;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using GameScripts.Misc;

namespace GameScripts.UI
{
    public class FieldView : MonoBehaviour
    {
        [SerializeField] private CellView cellViewPrefab;
        [SerializeField] private RectTransform cellContainerRect;
        [SerializeField] private GridLayoutGroup gridLayout;
        [SerializeField] private ActiveShapeContainer shapeContainer;

        private FieldViewModelContainer _fieldViewModelContainer;
        private ShapeViewModelsContainer _shapeViewModelsContainer;
        private FieldViewModel _fieldViewModel;
        private CellView[,] _cellViews;
        private List<Vector2Int> _shadowedCells;
        private CompositeDisposable _disposables;

        [Inject]
        public void Construct(FieldViewModelContainer fieldViewModelContainer, ShapeViewModelsContainer shapeViewModelsContainer)
        {
            _fieldViewModelContainer = fieldViewModelContainer;
            _shapeViewModelsContainer = shapeViewModelsContainer;
        }
        
        private void Awake()
        {
            _cellViews = new CellView[9, 9];
            _disposables = new CompositeDisposable();
            _shadowedCells = new List<Vector2Int>();
        }

        private void Start()
        {
            _fieldViewModelContainer.FieldViewModel.Subscribe(Initialize).AddTo(_disposables);
            shapeContainer.HoveredCell
                .DistinctUntilChanged()
                .Subscribe(OnHoveredCellChanged).AddTo(_disposables);
            SetupCells();
            Initialize(_fieldViewModelContainer.FieldViewModel.Value);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        private void Initialize(FieldViewModel fieldViewModel)
        {
            _fieldViewModel = fieldViewModel;
        }

        private void SetupCells()
        {
            cellContainerRect.DestroyAllChildren();
            
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

        private void OnHoveredCellChanged(Vector2Int hoveredCell)
        {
            if (hoveredCell == new Vector2Int(-1, -1))
            {
                CancelAllShadowing();
                return;
            }
            var shapeViewModel = _shapeViewModelsContainer.shapeViewModels[shapeContainer.SelectedShapeNumber];

            if (_fieldViewModel.PreviewShapePlacement(shapeViewModel.Uid,
                shapeViewModel.Rotation.Value,
                hoveredCell,
                out List<Vector2Int> occupiedCells))
            {
                AnimateCellPreviewForPlacement(occupiedCells);
            }
            else
            {
                CancelAllShadowing();
            }
        }

        private void AnimateCellPreviewForPlacement(List<Vector2Int> shadowedCells)
        {
            CancelAllShadowing();
            foreach (var cell in shadowedCells)
            {
                _shadowedCells.Add(cell);
                _cellViews[cell.x, cell.y].AnimateShadow();
            }
        }

        private void CancelAllShadowing()
        {
            foreach (var cell in _shadowedCells)
            {
                _cellViews[cell.x, cell.y].AnimateNormal();
            }
            _shadowedCells.Clear();
        }

        private void InstantiateCellViewAt(int x, int y)
        {
            var cellView = Instantiate(cellViewPrefab, cellContainerRect);
            _cellViews[x, y] = cellView;
        }
    }
}