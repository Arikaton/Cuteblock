using DG.Tweening;
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
            if (shapeView.ShapeUid == -1)
            {
                shapeView.transform.SetParent(shapeView.gemsAnimationTarget);
                var sequence = DOTween.Sequence();
                float duration = Random.Range(1.4f, 2f);
                sequence.Insert(0.0f, shapeView.containerRect.DOAnchorPos(Vector2.zero, duration).SetEase(Ease.InOutBack));
                var scaleRatio = shapeView.gemsAnimationTarget.sizeDelta.x / shapeView.shapeRect.sizeDelta.x;
                sequence.Insert(0.0f, shapeView.containerRect.DOScale(new Vector3(scaleRatio, scaleRatio, scaleRatio), duration));
                sequence.OnComplete(() => Object.Destroy(shapeView.gameObject));
            }
            else
            {
                Object.Destroy(shapeView.gameObject);
            }
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