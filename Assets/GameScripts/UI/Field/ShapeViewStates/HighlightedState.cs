using UnityEngine.EventSystems;

namespace GameScripts.UI
{
    public class HighlightedState : ShapeView.ShapeViewState
    {
        public HighlightedState(ShapeView shapeView) : base(shapeView)
        {
        }

        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
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