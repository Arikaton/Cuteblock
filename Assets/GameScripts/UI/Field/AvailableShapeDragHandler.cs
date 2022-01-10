using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScripts.Game
{
    public class AvailableShapeDragHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler
    {
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
            var offset = activeShapeContainer.containerRect.position - transform.position;
            offset.x = 0;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _mainCanvas.GetComponent<RectTransform>(), eventData.position, null, out Vector3 worldPoint);
            activeShapeContainer.containerRect.position = worldPoint + offset;
            activeShapeContainer.AnimatePopUp();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.pointerId != 0 && eventData.pointerId != -1) 
                eventData.pointerDrag = null;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
            activeShapeContainer.containerRect.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            activeShapeContainer.AnimateHide();
        }
    }
}