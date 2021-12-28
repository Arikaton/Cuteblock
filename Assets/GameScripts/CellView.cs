using GameScripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScripts
{
    public class CellView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private CellAnimator _cellAnimator;

        private void Awake()
        {
            _cellAnimator = GetComponent<CellAnimator>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _cellAnimator.AnimateHighlight();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _cellAnimator.AnimateClear();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _cellAnimator.AnimateBusy();
        }
    }
}