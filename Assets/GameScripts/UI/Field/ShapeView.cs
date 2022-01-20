using System;
using DG.Tweening;
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
        private const float ShapeStartingOffset = 0.2f;
        private const float AnimationSpeed = 0.15f;
        
        public RectTransform containerRect;
        public RectTransform shapeRect;
        public Image shapeImage;

        private IReactiveProperty<Vector2Int> _hoveredCell;
        private ShapeViewModel _viewModel;
        private IShapeSpritesProvider _shapeSpritesProvider;
        private FieldView _fieldView;
        private RectTransform _fieldRect;
        private Canvas _mainCanvas;
        private int _shapeIndex;
        private bool _placedOnField;
        private float _cellSize;
        private Sequence _sequence;
        private bool _hovering;
        private bool _available;
        private CompositeDisposable _disposables = new CompositeDisposable();

        private bool DragAvailable => !_placedOnField && _available;

        public void Initialize(RectTransform fieldRect, IShapeSpritesProvider shapeSpritesProvider, int shapeIndex,
            FieldView fieldView)
        {
            _fieldRect = fieldRect;
            _shapeSpritesProvider = shapeSpritesProvider;
            _shapeIndex = shapeIndex;
            _fieldView = fieldView;
            _hoveredCell = new ReactiveProperty<Vector2Int>(new Vector2Int(-1, -1));
            _cellSize = _fieldRect.sizeDelta.x / 9;
            containerRect.anchoredPosition = Vector2.zero;
            _mainCanvas = GetComponentInParent<Canvas>().rootCanvas;
            shapeRect.localScale = new Vector3(0.6f, 0.6f, 1f);
            _hoveredCell.DistinctUntilChanged().Subscribe(HoveredCellChanged).AddTo(_disposables);
        }

        public void Bind(ShapeViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.PositionOnGrid.Subscribe(SnapToPositionOnGrid).AddTo(_disposables);
            _viewModel.CanBePlaced.Subscribe(SwitchAvailability).AddTo(_disposables);
            LoadSprite();
        }

        private void Update()
        {
            _hoveredCell.Value = CalculateShapeOriginInField();
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        private Vector2Int CalculateShapeOriginInField()
        {
            if (!_hovering)
            {
                return new Vector2Int(-1, -1);
            }
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_fieldRect,
                GetRayOrigin(),
                null, out var shapeOriginInField);

            var shapeOriginInFieldNormalized =
                (shapeOriginInField + new Vector2(_fieldRect.rect.width * 0.5f, _fieldRect.rect.height * 0.5f))
                / (_fieldRect.rect.width / 9);

            if (shapeOriginInFieldNormalized.x < -0.49f || shapeOriginInFieldNormalized.x >= 9 ||
                shapeOriginInFieldNormalized.y < -0.49f || shapeOriginInFieldNormalized.y >= 9)
            {
                return new Vector2Int(-1, -1);
            }

            var currentPlacementPosition = FindCurrentPlacementPosition(shapeOriginInFieldNormalized, _viewModel.Rotation.Value);

            return currentPlacementPosition;
        }

        private static Vector2Int FindCurrentPlacementPosition(Vector2 shapeOriginInFieldNormalized, Rotation rotation)
        {
            var x = shapeOriginInFieldNormalized.x;
            var y = shapeOriginInFieldNormalized.y;
            switch (rotation)
            {
                case Rotation.Deg0:
                    return new Vector2Int(
                Math.Clamp((int) (x + 0.5f), 0, 8),
                Math.Clamp((int) (y + 0.5f), 0, 8)
            );
                case Rotation.Deg90:
                    return new Vector2Int(
                Math.Clamp((int) (x + 0.5f) - 1, 0, 8),
                Math.Clamp((int) (y + 0.5f), 0, 8)
            );
                case Rotation.Deg180:
                    return new Vector2Int(
                Math.Clamp((int) (x + 0.5f) - 1, 0, 8),
                Math.Clamp((int) (y + 0.5f) - 1, 0, 8)
            );
                case Rotation.Deg270:
                    return new Vector2Int(
                Math.Clamp((int) (x + 0.5f), 0, 8),
                Math.Clamp((int) (y + 0.5f) - 1, 0, 8)
            );
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
            }
        }

        private void LoadSprite()
        {
            shapeImage.sprite = _shapeSpritesProvider.GetShapeSprite(_viewModel.Uid);
            shapeRect.sizeDelta = new Vector2(_viewModel.Rect.x * _cellSize, _viewModel.Rect.y * _cellSize);
            containerRect.eulerAngles = new Vector3(containerRect.eulerAngles.x, containerRect.eulerAngles.y, _viewModel.Rotation.Value.AngleValue());
        }

        private void SnapToPositionOnGrid(Vector2Int cell)
        {
            if (cell == new Vector2Int(-1, -1))
                return;
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            containerRect.SetParent(_fieldRect);
            containerRect.anchoredPosition = FindAnchoredPositionOnField(cell);
            _placedOnField = true;
            _sequence.Insert(0.0f, shapeRect.DOAnchorPos(Vector2.zero, AnimationSpeed).SetEase(Ease.InOutQuad));
            _sequence.Insert(0.0f, shapeRect.DOScale(Vector2.one, AnimationSpeed).SetEase(Ease.InOutQuad));
        }

        private Vector2 FindAnchoredPositionOnField(Vector2Int cell)
        {
            var originCellCenter = (Vector2) cell * _cellSize -
                                   new Vector2(_fieldRect.rect.width * 0.5f, _fieldRect.rect.height * 0.5f) +
                                   new Vector2(_cellSize * 0.5f, _cellSize * 0.5f);
            return originCellCenter + _viewModel.OriginCenterToShapeCenterDistanceNormalized() * _cellSize;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(!DragAvailable)
                return;
            if (eventData.pointerId != 0 && eventData.pointerId != -1)
                return;

            var offset = _mainCanvas.pixelRect.height * ShapeStartingOffset;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _mainCanvas.GetComponent<RectTransform>(), eventData.position, null, out Vector3 worldPoint);
            
            shapeRect.SetParent(_mainCanvas.transform);
            containerRect.position = worldPoint + new Vector3(0f, offset);
            shapeRect.SetParent(containerRect);
            
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Insert(0.0f, shapeRect.DOAnchorPos(Vector2.zero, AnimationSpeed).SetEase(Ease.InOutQuad));
            _sequence.Insert(0.0f, shapeRect.DOScale(Vector2.one, AnimationSpeed).SetEase(Ease.InOutQuad));
            _hovering = true;

            _fieldView.CurrentShapeIndex = _shapeIndex;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!DragAvailable || (eventData.pointerId != 0 && eventData.pointerId != -1)) 
                eventData.pointerDrag = null;
        }

        public void OnDrag(PointerEventData eventData)
        {
            containerRect.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!DragAvailable)
                return;
            _hovering = false;
            
            if (_fieldView.TryPlaceShape(_hoveredCell.Value))
            {
                _fieldView.CurrentShapeIndex = -1;
                return;
            }
            
            shapeRect.SetParent(_mainCanvas.transform);
            containerRect.anchoredPosition = Vector2.zero;
            shapeRect.SetParent(containerRect);
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Insert(0.0f, shapeRect.DOAnchorPos(Vector2.zero, AnimationSpeed).SetEase(Ease.InOutQuad));
            _sequence.Insert(0.0f, shapeRect.DOScale(new Vector3(0.6f, 0.6f, 1f), AnimationSpeed).SetEase(Ease.InOutQuad));
        }

        private Vector2 GetRayOrigin()
        {
            var corners = new Vector3[4];
            shapeRect.GetWorldCorners(corners);
            return corners[0];
        }

        private void HoveredCellChanged(Vector2Int cell)
        {
            _fieldView.OnHoveredCellChanged(cell);
        }

        private void SwitchAvailability(bool available)
        {
            _available = available;
            shapeImage.DOFade(available ? 1f : 0.5f, 0.2f);
        }
    }
}