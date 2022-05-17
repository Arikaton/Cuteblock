using DG.Tweening;
using UnityEngine;

namespace GameScripts.UI
{
    public class HandAnimation : MonoBehaviour
    {
        public RectTransform rectTransform;
        public Vector2 startAnchorPos;
        public Vector2 endAnchorPos;
        
        private void Start()
        {
            rectTransform.anchoredPosition = startAnchorPos;
            rectTransform.DOAnchorPos(endAnchorPos, 1.5f).SetLoops(-1).SetEase(Ease.InOutQuad);
        }
    }
}
