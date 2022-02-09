using GameScripts.HapticFeedback;
using GameScripts.Providers;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GameScripts.UI
{
    public class HapticElement : MonoBehaviour, IPointerDownHandler
    {
        private ISoundAndHapticSettingsProvider _settingsProvider;
        
        [SerializeField] private FeedbackType _feedbackType = FeedbackType.MediumImpact;

        [Inject]
        public void Construct(ISoundAndHapticSettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_settingsProvider.Haptic.Value)
                return;
            HapticFeedbackGenerator.Haptic(_feedbackType);
        }
    }
}