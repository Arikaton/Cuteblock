using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UI
{
    public class SettingsAnimator : MonoBehaviour
    {
        [SerializeField] private RectTransform container;
        [SerializeField] private Canvas[] childrenCanvases;
        [SerializeField] private GraphicRaycaster[] childrenRaycasters;
        
        [SerializeField] private float startHeight;
        [SerializeField] private float endHeight;
        [SerializeField] private float duration;
        [SerializeField] private Ease ease;

        private Sequence _sequence;

        public void Animate(bool opened)
        {
            if (opened)
                Show();
            else 
                Hide();
        }

        public void Show()
        {
            foreach (var childrenCanvas in childrenCanvases)
            {
                childrenCanvas.enabled = true;
            }

            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(
                container
                    .DOSizeDelta(new Vector2(container.rect.width, endHeight), duration)
                    .SetEase(ease)
                    .OnComplete(() =>
                    {
                        foreach (var raycaster in childrenRaycasters)
                        {
                            raycaster.enabled = true;
                        }
                    }));
        }
        
        public void Hide()
        {
            foreach (var raycaster in childrenRaycasters)
            {
                raycaster.enabled = false;
            }

            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(
                container
                    .DOSizeDelta(new Vector2(container.rect.width, startHeight), duration)
                    .SetEase(ease)
                    .OnComplete(() =>
                    {
                        foreach (var childrenCanvas in childrenCanvases)
                        {
                            childrenCanvas.enabled = false;
                        }
                    }));
        }
    }
}