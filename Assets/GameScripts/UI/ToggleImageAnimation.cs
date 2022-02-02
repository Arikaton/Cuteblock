using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameScripts.UI
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleImageAnimation : MonoBehaviour
    {
        private Toggle _toggle;
        private CompositeDisposable _disposables;

        [SerializeField] private Image targetImage;
        [SerializeField] private Sprite isOnSprite;
        [SerializeField] private Sprite isOffSprite;

        private void Awake()
        {
            _disposables = new CompositeDisposable();
            _toggle = GetComponent<Toggle>();
            _toggle.OnValueChangedAsObservable().Subscribe(Animate).AddTo(_disposables);
        }

        private void Animate(bool isOn)
        {
            if (isOn)
                AnimateOn();
            else
                AnimateOff();
        }

        private void AnimateOn()
        {
            targetImage.sprite = isOnSprite;
        }

        private void AnimateOff()
        {
            targetImage.sprite = isOffSprite;
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}