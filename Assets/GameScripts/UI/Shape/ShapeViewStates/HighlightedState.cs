using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScripts.UI
{
    public class HighlightedState : ShapeView.ShapeViewState
    {
        private Sequence _sequence;
        
        public HighlightedState(ShapeView shapeView) : base(shapeView)
        {
        }

        public override void OnEnter()
        {
            _sequence = DOTween.Sequence();
            _sequence.Insert(0.0f, shapeView.shapeRect.DOAnchorPos(Vector2.zero, AnimationDuration).SetEase(Ease.InOutQuad));
            _sequence.Insert(0.0f,
                viewModel.PositionOnGrid.Value != new Vector2(-1, -1)
                    ? shapeView.shapeRect.DOScale(Vector3.one, AnimationDuration).SetEase(Ease.InOutQuad)
                    : shapeView.shapeRect.DOScale(new Vector3(0.6f, 0.6f, 1f), AnimationDuration).SetEase(Ease.InOutQuad));
        }

        public override void OnExit()
        {
            _sequence?.Kill();
        }

        public override void Update()
        {
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            shapeView.Click();
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
    }
}