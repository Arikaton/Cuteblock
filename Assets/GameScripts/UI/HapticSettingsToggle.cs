using GameScripts.Providers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameScripts.UI
{
    public class HapticSettingsToggle : MonoBehaviour
    {
        private ISoundAndHapticSettingsProvider _settingsProvider;

        public Toggle toggle;

        [Inject]
        public void Construct(ISoundAndHapticSettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        private void Start()
        {
            toggle.isOn = _settingsProvider.Haptic.Value;
            toggle.OnValueChangedAsObservable().Subscribe(value => _settingsProvider.Haptic.Value = value).AddTo(this);
        }
    }
}