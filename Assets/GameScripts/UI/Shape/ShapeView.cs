using GameScripts.Game;
using GameScripts.Providers;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameScripts.UI
{
    public class ShapeView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler
    {
        public RectTransform containerRect;
        public RectTransform shapeRect;
        public Image shapeImage;
        public RectTransform rotationLabel;
        public Image containerRectImage;
        [HideInInspector]
        public RectTransform gemsAnimationTarget;
        
        private ShapeViewModel _viewModel;
        private IShapeSpritesProvider _shapeSpritesProvider;
        private FieldView _fieldView;
        private RectTransform _shapesContainer;
        private Canvas _mainCanvas;
        private float _cellSize;
        private ShapeViewState _currentState;
        private bool _stateChanged;

        public int ShapeIndex { get; private set; }
        public int ShapeUid => _viewModel.Uid;

        private void Awake()
        {
            ChangeState(new InitializingState(this));
        }

        public void Initialize(RectTransform shapesContainer, IShapeSpritesProvider shapeSpritesProvider, int shapeIndex,
            FieldView fieldView, RectTransform gemsAnimationTgt)
        {
            _shapesContainer = shapesContainer;
            _shapeSpritesProvider = shapeSpritesProvider;
            ShapeIndex = shapeIndex;
            _fieldView = fieldView;
            gemsAnimationTarget = gemsAnimationTgt;
            _cellSize = _shapesContainer.sizeDelta.x / 9;
            containerRect.anchoredPosition = Vector2.zero;
            _mainCanvas = GetComponentInParent<Canvas>().rootCanvas;
            shapeRect.localScale = new Vector3(0.6f, 0.6f, 6f);
        }

        public void Bind(ShapeViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.PositionOnGrid.SkipLatestValueOnSubscribe().Subscribe(SnapToPositionOnGrid).AddTo(this);
            _viewModel.CanBePlaced.SkipLatestValueOnSubscribe().Subscribe(SwitchAvailability).AddTo(this);
            _viewModel.Destroy.Subscribe(_ => DestroyShape()).AddTo(this);
            _viewModel.Rotation.Subscribe(ChangeRotation).AddTo(this);
            _viewModel.Highlighted.SkipLatestValueOnSubscribe().Subscribe(SwitchHighlighting).AddTo(this);
            LoadSprite();

            if (_viewModel.PositionOnGrid.Value != new Vector2Int(-1, -1))
                ChangeState(new PlacedOnFieldState(this));
            else
                ChangeState(new ActiveState(this));
        }

        private void DestroyShape()
        {
            ChangeState(new DestroyingState(this));
        }

        public void ChangeState(ShapeViewState state)
        {
            _currentState?.OnExit();
            _currentState = state;
            _currentState.OnEnter();
            _stateChanged = true;
        }

        private void LoadSprite()
        {
            shapeImage.sprite = _shapeSpritesProvider.GetShapeSprite(_viewModel.Uid);
            shapeRect.sizeDelta = new Vector2(_viewModel.Rect.x * _cellSize, _viewModel.Rect.y * _cellSize);
            containerRect.eulerAngles = new Vector3(containerRect.eulerAngles.x, containerRect.eulerAngles.y, _viewModel.Rotation.Value.AngleValue());
        }

        private void Update()
        {
            _currentState.Update();
        }

        private void ChangeRotation(Rotation rotation)
        {
            containerRect.eulerAngles = new Vector3(containerRect.eulerAngles.x, containerRect.eulerAngles.y, rotation.AngleValue());
        }

        private void SnapToPositionOnGrid(Vector2Int cell)
        {
            if (cell == new Vector2Int(-1, -1))
                return;
            ChangeState(new PlacedOnFieldState(this));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_stateChanged) _stateChanged = false;
            _currentState.OnPointerDown(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_stateChanged) return;
            _currentState.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_stateChanged) return;
            _currentState.OnDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_stateChanged) return;
            _currentState.OnPointerUp(eventData);
        }

        private void SwitchAvailability(bool available)
        {
            if(available)
                ChangeState(new ActiveState(this));
            else
                ChangeState(new InactiveState(this));
        }
        
        private void SwitchHighlighting(bool highlighted)
        {
            if (highlighted)
            {
                ChangeState(new HighlightedState(this));
                return;
            }

            if (_viewModel.PositionOnGrid.Value != new Vector2Int(-1, -1))
                ChangeState(new PlacedOnFieldState(this));
            else
            {
                if(_viewModel.CanBePlaced.Value)
                    ChangeState(new ActiveState(this));
                else
                    ChangeState(new InactiveState(this));
            }
        }

        public void Click()
        {
            _viewModel.Click();
        }

        public abstract class ShapeViewState
        {
            protected const float ShapeStartingOffset = 0.13f;
            protected const float AnimationDuration = 0.15f;
            
            protected ShapeView shapeView;
            protected Canvas mainCanvas;
            protected FieldView fieldView;
            protected ShapeViewModel viewModel;
            protected RectTransform shapesContainer;
            protected float cellSize;

            public ShapeViewState(ShapeView shapeView)
            {
                this.shapeView = shapeView;
                fieldView = shapeView._fieldView;
                mainCanvas = shapeView._mainCanvas;
                viewModel = shapeView._viewModel;
                shapesContainer = shapeView._shapesContainer;
                cellSize = shapeView._cellSize;
            }

            public abstract void OnEnter();
            public abstract void OnExit();
            public abstract void Update();
            public abstract void OnPointerDown(PointerEventData eventData);
            public abstract void OnBeginDrag(PointerEventData eventData);
            public abstract void OnDrag(PointerEventData eventData);
            public abstract void OnPointerUp(PointerEventData eventData);
        }
    }
}