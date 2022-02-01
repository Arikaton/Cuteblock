using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScripts.UI
{
    public class PlacedOnFieldState : ShapeView.ShapeViewState
    {
        private Sequence _sequence;
        private CompositeDisposable _disposables;

        public PlacedOnFieldState(ShapeView shapeView) : base(shapeView)
        {
        }

        public override void OnEnter()
        {
            _sequence = DOTween.Sequence();
            shapeView.containerRect.SetParent(shapesContainer);
            shapeView.containerRect.anchoredPosition = FindAnchoredPositionOnField(viewModel.PositionOnGrid.Value);
            _sequence.Insert(0.0f, shapeView.shapeRect.DOAnchorPos(Vector2.zero, AnimationSpeed).SetEase(Ease.InOutQuad));
            _sequence.Insert(0.0f, shapeView.shapeRect.DOScale(Vector2.one, AnimationSpeed).SetEase(Ease.InOutQuad));

            _disposables = new CompositeDisposable();
        }

        public override void OnExit()
        {
            _sequence?.Kill();
            _disposables.Dispose();
        }

        public override void Update()
        {
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
        }

        public override void OnDrag(PointerEventData eventData)
        {
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
        }

        private Vector2 FindAnchoredPositionOnField(Vector2Int cell)
        {
            var originCellCenter = (Vector2) cell * cellSize -
                                   new Vector2(shapesContainer.rect.width * 0.5f, shapesContainer.rect.height * 0.5f) +
                                   new Vector2(cellSize * 0.5f, cellSize * 0.5f);
            return originCellCenter + viewModel.OriginCenterToShapeCenterDistanceNormalized() * cellSize;
        }
    }
}