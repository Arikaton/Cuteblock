using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScripts.UI
{
    public class NormalState : ShapeView.ShapeViewState
    {
        private const float ShapeStartingOffset = 0.13f;
        private const float AnimationSpeed = 0.15f;
        
        private Sequence _sequence;

        public NormalState(ShapeView shapeView) : base(shapeView)
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
            if (eventData.pointerId != 0 && eventData.pointerId != -1)
                return; ;

            var offset = mainCanvas.pixelRect.height * ShapeStartingOffset;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                mainCanvas.GetComponent<RectTransform>(), eventData.position, null, out Vector3 worldPoint);
            
            shapeView.shapeRect.SetParent(mainCanvas.transform);
            shapeView.containerRect.position = worldPoint + new Vector3(0f, offset);
            shapeView.shapeRect.SetParent(shapeView.containerRect);
            
            _sequence = DOTween.Sequence();
            _sequence.Insert(0.0f, shapeView.shapeRect.DOAnchorPos(Vector2.zero, AnimationSpeed).SetEase(Ease.InOutQuad));
            _sequence.Insert(0.0f, shapeView.shapeRect.DOScale(Vector2.one, AnimationSpeed).SetEase(Ease.InOutQuad));
            
            // _fieldView.CurrentShapeIndex = _shapeIndex;
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
            if (shapeView._fieldView.TryPlaceShape(shapeView._hoveredCell.Value))
            {
                shapeView._fieldView.CurrentShapeIndex = -1;
                return;
            }
            
            shapeView.shapeRect.SetParent(mainCanvas.transform);
            shapeView.containerRect.anchoredPosition = Vector2.zero;
            shapeView.shapeRect.SetParent(shapeView.containerRect);
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Insert(0.0f, shapeView.shapeRect.DOAnchorPos(Vector2.zero, AnimationSpeed).SetEase(Ease.InOutQuad));
            _sequence.Insert(0.0f, shapeView.shapeRect.DOScale(new Vector3(0.6f, 0.6f, 1f), AnimationSpeed).SetEase(Ease.InOutQuad));
        }
    }
}