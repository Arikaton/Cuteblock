using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameScripts.UI
{
    public class HighlightedState : ShapeView.ShapeViewState
    {
        private Sequence _sequence;
        private Sequence _rotationLabelSequence;
        
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

            if (viewModel.PositionOnGrid.Value == new Vector2Int(-1, -1))
            {
                shapeView.rotationLabel.gameObject.SetActive(true);
                _rotationLabelSequence = DOTween.Sequence();
                _rotationLabelSequence.Append(shapeView.rotationLabel
                    .DOBlendableLocalRotateBy(new Vector3(0, 0, -130), 0.7f, RotateMode.FastBeyond360)
                    .SetEase(Ease.OutQuart));
                _rotationLabelSequence.Insert(0.0f,
                    shapeView.rotationLabel.DOBlendableLocalRotateBy(new Vector3(0, 0, -360), 12, RotateMode.FastBeyond360)
                        .SetRelative()
                        .SetEase(Ease.Linear)
                        .SetLoops(1000, LoopType.Incremental));
            }
        }

        public override void OnExit()
        {
            _sequence?.Kill();
            _rotationLabelSequence?.Kill();
            shapeView.rotationLabel.gameObject.SetActive(false);
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