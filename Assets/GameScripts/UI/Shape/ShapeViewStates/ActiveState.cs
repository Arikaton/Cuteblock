using System;
using DG.Tweening;
using GameScripts.Game;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScripts.UI
{
    public class ActiveState : ShapeView.ShapeViewState
    {
        private IReactiveProperty<Vector2Int> _hoveredCell;
        private Sequence _sequence;
        private CompositeDisposable _disposables;

        public ActiveState(ShapeView shapeView) : base(shapeView)
        {
        }

        public override void OnEnter()
        {
            _disposables = new CompositeDisposable();
            _hoveredCell = new ReactiveProperty<Vector2Int>(new Vector2Int(-1, -1));
            _hoveredCell.DistinctUntilChanged().Subscribe(ChangeHoveredCell).AddTo(_disposables);
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Insert(0.0f, shapeView.shapeRect.DOScale(new Vector3(0.6f, 0.6f, 1f), AnimationDuration).SetEase(Ease.OutQuad));
        }

        public override void OnExit()
        {
            _hoveredCell.Value = new Vector2Int(-1, -1);
            _sequence?.Kill();
            _disposables.Dispose();
        }

        public override void Update()
        {
            _hoveredCell.Value = CalculateShapeOriginInField();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.pointerId != 0 && eventData.pointerId != -1)
                return;
            
            shapeView.containerRect.transform.parent.SetAsLastSibling();

            var offset = mainCanvas.pixelRect.height * ShapeStartingOffset;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                mainCanvas.GetComponent<RectTransform>(), eventData.position, null, out Vector3 worldPoint);
            
            shapeView.shapeRect.SetParent(mainCanvas.transform);
            shapeView.containerRect.position = worldPoint + new Vector3(0f, offset);
            shapeView.shapeRect.SetParent(shapeView.containerRect);
            
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Insert(0.0f, shapeView.shapeRect.DOAnchorPos(Vector2.zero, AnimationDuration).SetEase(Ease.InOutQuad));
            _sequence.Insert(0.0f, shapeView.shapeRect.DOScale(Vector3.one, AnimationDuration).SetEase(Ease.InOutQuad));
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.pointerId != 0 && eventData.pointerId != -1) 
                eventData.pointerDrag = null;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            shapeView.containerRect.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (fieldView.TryPlaceShape(_hoveredCell.Value, shapeView.ShapeIndex))
                return;

            shapeView.shapeRect.SetParent(mainCanvas.transform);
            shapeView.containerRect.anchoredPosition = Vector2.zero;
            shapeView.shapeRect.SetParent(shapeView.containerRect);
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Insert(0.0f, shapeView.shapeRect.DOAnchorPos(Vector2.zero, AnimationDuration).SetEase(Ease.InOutQuad));
            _sequence.Insert(0.0f, shapeView.shapeRect.DOScale(new Vector3(0.6f, 0.6f, 1f), AnimationDuration).SetEase(Ease.InOutQuad));
        }

        private Vector2Int CalculateShapeOriginInField()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(shapesContainer,
                GetRayOrigin(),
                null, out var shapeOriginInField);

            var shapeOriginInFieldNormalized =
                (shapeOriginInField + new Vector2(shapesContainer.rect.width * 0.5f, shapesContainer.rect.height * 0.5f))
                / (shapesContainer.rect.width / 9);

            if (shapeOriginInFieldNormalized.x < -0.49f || shapeOriginInFieldNormalized.x >= 9.5f ||
                shapeOriginInFieldNormalized.y < -0.49f || shapeOriginInFieldNormalized.y >= 9.5f)
            {
                return new Vector2Int(-1, -1);
            }

            var currentPlacementPosition = FindCurrentPlacementPosition(shapeOriginInFieldNormalized, viewModel.Rotation.Value);

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

        private Vector2 GetRayOrigin()
        {
            var corners = new Vector3[4];
            shapeView.shapeRect.GetWorldCorners(corners);
            return corners[0];
        }

        private void ChangeHoveredCell(Vector2Int cell)
        {
            fieldView.ChangeHoveredCell(cell, shapeView.ShapeIndex);
        }
    }
}