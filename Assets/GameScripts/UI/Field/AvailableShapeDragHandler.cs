using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScripts.UI
{
    public class AvailableShapeDragHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler
    {
        [SerializeField] [Range(0, 2)] private int shapeNumber;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private ActiveShapeContainer activeShapeContainer;
        private Canvas _mainCanvas;

        private void Awake()
        {
            _mainCanvas = GetComponentInParent<Canvas>().rootCanvas;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.pointerId != 0 && eventData.pointerId != -1)
                return;
            
            activeShapeContainer.ResetAnchoredPosition();
            var offset = activeShapeContainer.activeShapeRect.position - transform.position;
            offset.x = 0;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _mainCanvas.GetComponent<RectTransform>(), eventData.position, null, out Vector3 worldPoint);
            activeShapeContainer.activeShapeRect.position = worldPoint + offset;
            activeShapeContainer.AnimatePopUp(shapeNumber);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.pointerId != 0 && eventData.pointerId != -1) 
                eventData.pointerDrag = null;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
            activeShapeContainer.activeShapeRect.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            activeShapeContainer.AnimateHide();
        }
    }
}