using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScripts.UI
{
    public class DestroyingState : ShapeView.ShapeViewState
    {
        public DestroyingState(ShapeView shapeView) : base(shapeView)
        {
        }

        public override void OnEnter()
        {
            Object.Destroy(shapeView.gameObject);
        }

        public override void OnExit()
        {
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