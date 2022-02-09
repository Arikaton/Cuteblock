using UniRx;
using UnityEngine;

namespace GameScripts.Providers
{
    public class SoundAndHapticSettingsProvider : ISoundAndHapticSettingsProvider
    {
        private const string HAPTIC_PREFS_KEY = "Haptic";
        private const string SOUND_PREFS_KEY = "Sound";

        public IReactiveProperty<bool> Haptic { get; }
        public IReactiveProperty<bool> Sound { get; }
        
        private CompositeDisposable _disposables;

        public SoundAndHapticSettingsProvider()
        {
            _disposables = new CompositeDisposable();
            Haptic = new ReactiveProperty<bool>();
            Sound = new ReactiveProperty<bool>();
            
            Haptic.Value = PlayerPrefs.GetInt(HAPTIC_PREFS_KEY) == 1;
            Sound.Value = PlayerPrefs.GetInt(SOUND_PREFS_KEY) == 1;
            Haptic.Subscribe(value => PlayerPrefs.SetInt(HAPTIC_PREFS_KEY, value ? 1 : 0)).AddTo(_disposables);
            Sound.Subscribe(value => PlayerPrefs.SetInt(SOUND_PREFS_KEY, value ? 1 : 0)).AddTo(_disposables);
        }

        ~SoundAndHapticSettingsProvider()
        {
            _disposables.Dispose();
        }
    }
}