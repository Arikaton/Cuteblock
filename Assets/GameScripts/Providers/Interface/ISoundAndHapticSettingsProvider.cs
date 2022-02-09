using UniRx;

namespace GameScripts.Providers
{
    public interface ISoundAndHapticSettingsProvider
    {
        IReactiveProperty<bool> Haptic { get; }
        IReactiveProperty<bool> Sound { get; }
    }
}