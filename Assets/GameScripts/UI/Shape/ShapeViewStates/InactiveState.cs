using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScripts.UI
{
    public class InactiveState : ShapeView.ShapeViewState
    {
        private Sequence _sequence;
        public InactiveState(ShapeView shapeView) : base(shapeView)
        {
        }

        public override void OnEnter()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Insert(0f, shapeView.shapeImage.DOFade(0.5f, 0.13f));
        }

        public override void OnExit()
        {
            _sequence?.Kill();
            shapeView.shapeImage.color = new Color(1, 1, 1, 1);
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
    }
}