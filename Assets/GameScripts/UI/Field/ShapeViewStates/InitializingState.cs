using UnityEngine.EventSystems;

namespace GameScripts.UI
{
    public class InitializingState : ShapeView.ShapeViewState
    {
        public InitializingState(ShapeView shapeView) : base(shapeView)
        {
        }

        public override void OnEnter(){}

        public override void OnExit(){}

        public override void Update(){}

        public override void OnPointerDown(PointerEventData eventData){}

        public override void OnBeginDrag(PointerEventData eventData){}

        public override void OnDrag(PointerEventData eventData){}

        public override void OnPointerUp(PointerEventData eventData){}
    }
}