using System.Linq;
using DG.Tweening;
using GameScripts.Game;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using GameScripts.Misc;
using GameScripts.Providers;
using GameScripts.UIManagement;
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
        [SerializeField] private TextMeshProUGUI gemsCount;
        [SerializeField] private Transform backgroundShadowPanel;
        [SerializeField] private Transform availableShapesContainer;
        [SerializeField] private RectTransform gemsAnimationTarget;
        [SerializeField] private Image gemReferenceImage;

        [Space, Header("Hints")] 
        [SerializeField] private GameObject[] hints;
        [SerializeField] private GameObject hand;

        private FieldViewModelContainer _fieldViewModelContainer;
        private FieldViewModel _fieldViewModel;
        private IShapeSpritesProvider _shapeSpritesProvider;
        private CellView[,] _cellViews;
        private CompositeDisposable _tempDisposables = new CompositeDisposable();
        
        [Inject]
        public void Construct(FieldViewModelContainer fieldViewModelContainer, IShapeSpritesProvider shapeSpritesProvider)
        {
            _fieldViewModelContainer = fieldViewModelContainer;
            _fieldViewModelContainer.FieldViewModel.SkipLatestValueOnSubscribe().Subscribe(Initialize).AddTo(this);
            _shapeSpritesProvider = shapeSpritesProvider;
        }

        public bool TryPlaceShape(Vector2Int cell, int shapeIndex)
        {
            return _fieldViewModel.PlaceShape(shapeIndex, cell);
        }

        private void Awake()
        {
            _cellViews = new CellView[9, 9];
        }

        private void Initialize(FieldViewModel fieldViewModel)
        {
            CleanFromPreviousGame();
            _tempDisposables.Dispose();
            _tempDisposables = new CompositeDisposable();
            _fieldViewModel = fieldViewModel;
            _fieldViewModel.ShapesOnField.ObserveAdd().Subscribe(AddNewShapeOnField).AddTo(_tempDisposables);
            _fieldViewModel.AvailableShapes.ObserveReplace().Subscribe(AddNewAvailableShape).AddTo(_tempDisposables);
            _fieldViewModel.GemsLeftToCollect.Subscribe(UpdateGemsCount).AddTo(_tempDisposables);
            _fieldViewModel.HighlightAvailableShapes.Subscribe(SwitchAvailableShapesHighlighting).AddTo(_tempDisposables);
            _fieldViewModel.HighlightShapesOnField.Subscribe(SwitchShapesOnFieldHighlighting).AddTo(_tempDisposables);
            _fieldViewModel.OnGameWon.Subscribe(_ => ShowWinPopup()).AddTo(_tempDisposables);
            _fieldViewModel.OnGameLost.Subscribe(_ => ShowOutOfMovesPopup()).AddTo(_tempDisposables);

            foreach (var shapeOnField in _fieldViewModel.ShapesOnField)
            {
                var shapeView = CreateShapeView(0);
                shapeView.Bind(shapeOnField);
            }

            for (int i = 0; i < _fieldViewModel.AvailableShapes.Count; i++)
            {
                if(_fieldViewModel.AvailableShapes[i] == null) continue;
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

            var gemsShapeId = _fieldViewModel.ShapesOnField.Select(x => x.Uid).First(x => x < 0);
            gemReferenceImage.sprite = _shapeSpritesProvider.GetShapeSprite(gemsShapeId);

            ActivateHints();
        }

        private void ActivateHints()
        {
            hand.SetActive(_fieldViewModel.Level <= 3);
            for (var i = 0; i < hints.Length; i++)
            {
                var hint = hints[i];
                hint.SetActive(_fieldViewModel.Level == i + 1);
            }
            foreach (var hint in hints)
            {
                hint.transform.Find("Mask").gameObject.SetActive(true);
            }
        }

        private void ShowWinPopup()
        {
            foreach (var particle in FindObjectsOfType<ParticleSystem>())
            {
                particle.Play();
            }
            UIManager.Instance.ShowPopup(UIViewId.PopupLevelCompleted);
            hand.SetActive(false);
            foreach (var hint in hints)
            {
                hint.transform.Find("Mask").gameObject.SetActive(false);
            }
        }
        
        private void ShowOutOfMovesPopup()
        {
            UIManager.Instance.ShowPopup(UIViewId.PopupOutOfMoves);
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
            shapeView.Initialize(shapesContainerRect, _shapeSpritesProvider, shapeIndex, this, gemsAnimationTarget);
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

        private void AddNewAvailableShape(CollectionReplaceEvent<ShapeViewModel> eventData)
        {
            if (eventData.NewValue == null) return;
            var shapeView = CreateShapeView(eventData.Index);
            shapeView.Bind(eventData.NewValue);
        }

        private void UpdateGemsCount(int count)
        {
            gemsCount.text = count.ToString();
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
    }
}