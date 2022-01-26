using DG.Tweening;
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

        private FieldViewModelContainer _fieldViewModelContainer;
        private FieldViewModel _fieldViewModel;
        private CellView[,] _cellViews;
        private CompositeDisposable _disposables;
        private ShapeViewFactory _shapeViewFactory;
        
        public int CurrentShapeIndex { get; set; }

        [Inject]
        public void Construct(FieldViewModelContainer fieldViewModelContainer, ShapeViewFactory shapeViewFactory)
        {
            _fieldViewModelContainer = fieldViewModelContainer;
            _shapeViewFactory = shapeViewFactory;
        }
        
        private void Awake()
        {
            _cellViews = new CellView[9, 9];
            _disposables = new CompositeDisposable();
        }

        private void Start()
        {
            SetupCells();
            _fieldViewModelContainer.FieldViewModel.Subscribe(Initialize).AddTo(_disposables);
            _fieldViewModel.OnAddNewShapeOnField.Subscribe(AddNewShapeOnField).AddTo(_disposables);
        }

        public bool TryPlaceShape(Vector2Int cell)
        {
            if (_fieldViewModel.PlaceShape(CurrentShapeIndex, cell))
            {
                Debug.Log("Placed shape");
                return true;
            }
            else
            {
                Debug.Log("Cannot place shape");
                return false;
            }
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        private void Initialize(FieldViewModel fieldViewModel)
        {
            _fieldViewModel = fieldViewModel;
            foreach (var shapeOnField in _fieldViewModel.shapesOnField)
            {
                var shapeView = _shapeViewFactory.CreateShapeView(0);
                shapeView.Bind(shapeOnField);
            }

            for (int i = 0; i < _fieldViewModel.availableShapes.Length; i++)
            {
                var shapeView = _shapeViewFactory.CreateShapeView(i);
                shapeView.Bind(_fieldViewModel.availableShapes[i]);
            }
            for (var x = 0; x < 9; x++)
            {
                for (var y = 0; y < 9; y++)
                {
                    _cellViews[x, y].Bind(_fieldViewModel.CellViewModels[x, y]);
                }
            }
        }

        private void SetupCells()
        {
            cellContainerRect.DestroyAllChildren();

            gridLayout.enabled = true;
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

            var sequence = DOTween.Sequence();
            sequence.PrependInterval(0.3f);
            sequence.AppendCallback(() => gridLayout.enabled = false);
        }

        private void InstantiateCellViewAt(int x, int y)
        {
            var cellView = Instantiate(cellViewPrefab, cellContainerRect);
            _cellViews[x, y] = cellView;
        }

        public void OnHoveredCellChanged(Vector2Int hoveredCell)
        {
            _fieldViewModel.PreviewShapePlacement(CurrentShapeIndex, hoveredCell);
        }

        private void AddNewShapeOnField((ShapeViewModel viewModel, int index) shape)
        {
            var shapeView = _shapeViewFactory.CreateShapeView(shape.index);
            shapeView.Bind(shape.viewModel);
        }
    }
}