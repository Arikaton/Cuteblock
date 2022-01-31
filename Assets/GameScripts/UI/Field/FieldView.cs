using DG.Tweening;
using GameScripts.Game;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using GameScripts.Misc;
using GameScripts.Providers;
using TMPro;

namespace GameScripts.UI
{
    public class FieldView : MonoBehaviour
    {
        [SerializeField] private CellView cellViewPrefab;
        [SerializeField] private ShapeView shapeViewPrefab;
        
        [SerializeField] private RectTransform cellsContainerRect;
        [SerializeField] private RectTransform shapesContainerRect;
        [SerializeField] private RectTransform[] availableFigureContainers = new RectTransform[3];
        [SerializeField] private GridLayoutGroup gridLayout;
        [SerializeField] private TextMeshProUGUI currentResult;
        [SerializeField] private Transform backgroundShadowPanel;
        [SerializeField] private Transform availableShapesContainer;
        
        [SerializeField] private Button rotateShapeButton;
        [SerializeField] private Button newShapesButton;
        [SerializeField] private Button removeShapeButton;

        private FieldViewModelContainer _fieldViewModelContainer;
        private FieldViewModel _fieldViewModel;
        private IShapeSpritesProvider _shapeSpritesProvider;
        private CellView[,] _cellViews;
        private CompositeDisposable _disposables;
        private CompositeDisposable _tempDisposables;
        
        [Inject]
        public void Construct(FieldViewModelContainer fieldViewModelContainer, IShapeSpritesProvider shapeSpritesProvider)
        {
            _fieldViewModelContainer = fieldViewModelContainer;
            _shapeSpritesProvider = shapeSpritesProvider;
        }

        public bool TryPlaceShape(Vector2Int cell, int shapeIndex)
        {
            return _fieldViewModel.PlaceShape(shapeIndex, cell);
        }

        private void Awake()
        {
            _cellViews = new CellView[9, 9];
            _disposables = new CompositeDisposable();
            _tempDisposables = new CompositeDisposable();
        }

        private void Start()
        {
            _fieldViewModelContainer.FieldViewModel.SkipLatestValueOnSubscribe().Subscribe(Initialize).AddTo(_disposables);
            rotateShapeButton.OnClickAsObservable().Subscribe(_ => BuyRotationHint()).AddTo(_disposables);
        }

        private void Initialize(FieldViewModel fieldViewModel)
        {
            CleanFromPreviousGame();
            _tempDisposables.Dispose();
            _tempDisposables = new CompositeDisposable();
            _fieldViewModel = fieldViewModel;
            _fieldViewModel.ShapesOnField.ObserveAdd().Subscribe(AddNewShapeOnField).AddTo(_tempDisposables);
            _fieldViewModel.AvailableShapes.ObserveAdd().Subscribe(AddNewAvailableShape).AddTo(_tempDisposables);
            _fieldViewModel.Score.Subscribe(UpdateScore).AddTo(_tempDisposables);
            _fieldViewModel.HighlightAvailableShapes.Subscribe(SwitchAvailableShapesHighlighting).AddTo(_tempDisposables);

            foreach (var shapeOnField in _fieldViewModel.ShapesOnField)
            {
                var shapeView = CreateShapeView(0);
                shapeView.Bind(shapeOnField);
            }

            for (int i = 0; i < _fieldViewModel.AvailableShapes.Count; i++)
            {
                var shapeView = CreateShapeView(i);
                shapeView.Bind(_fieldViewModel.AvailableShapes[i]);
            }
            for (var x = 0; x < 9; x++)
            {
                for (var y = 0; y < 9; y++)
                {
                    _cellViews[x, y].Bind(_fieldViewModel.CellViewModels[x, y]);
                }
            }
        }

        private void CleanFromPreviousGame()
        {
            cellsContainerRect.DestroyAllChildren();
            shapesContainerRect.DestroyAllChildren();
            foreach (var availableFigureContainer in availableFigureContainers)
                availableFigureContainer.DestroyAllChildren();
            SetupCells();
        }

        private void SetupCells()
        {
            gridLayout.enabled = true;
            gridLayout.cellSize = new Vector2(cellsContainerRect.rect.width / 9, cellsContainerRect.rect.height / 9);
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
            var cellView = Instantiate(cellViewPrefab, cellsContainerRect);
            _cellViews[x, y] = cellView;
        }

        private ShapeView CreateShapeView(int shapeIndex)
        {
            var shapeView = Instantiate(shapeViewPrefab, availableFigureContainers[shapeIndex]);
            shapeView.Initialize(shapesContainerRect, _shapeSpritesProvider, shapeIndex, this);
            return shapeView;
        }

        public void ChangeHoveredCell(Vector2Int hoveredCell, int shapeIndex)
        {
            _fieldViewModel.PreviewShapePlacement(shapeIndex, hoveredCell);
        }

        private void AddNewShapeOnField(CollectionAddEvent<ShapeViewModel> eventData)
        {
            if (eventData.Value == null) return;
            var shapeView = CreateShapeView(0);
            shapeView.Bind(eventData.Value);
        }

        private void AddNewAvailableShape(CollectionAddEvent<ShapeViewModel> eventData)
        {
            if (eventData.Value == null) return;
            var shapeView = CreateShapeView(eventData.Index);
            shapeView.Bind(eventData.Value);
        }

        private void UpdateScore(int score)
        {
            currentResult.text = score.ToString();
        }

        private void SwitchShapesOnFieldHighlighting(bool shadowing)
        {
            if (shadowing)
            {
                backgroundShadowPanel.SetAsLastSibling();
                shapesContainerRect.SetAsLastSibling();
                backgroundShadowPanel.gameObject.SetActive(true);
                return;
            }
            backgroundShadowPanel.gameObject.SetActive(false);
            shapesContainerRect.SetAsLastSibling();
            availableShapesContainer.SetAsLastSibling();
        }

        private void SwitchAvailableShapesHighlighting(bool shadowing)
        {
            if (shadowing)
            {
                backgroundShadowPanel.SetAsLastSibling();
                availableShapesContainer.SetAsLastSibling();
                backgroundShadowPanel.gameObject.SetActive(true);
                return;
            }
            backgroundShadowPanel.gameObject.SetActive(false);
            shapesContainerRect.SetAsLastSibling();
            availableShapesContainer.SetAsLastSibling();
        }

        private void BuyRotationHint()
        {
            _fieldViewModel.BuyRotateShape();
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}