using DG.Tweening;
using GameScripts.Providers;
using UnityEngine;
using Zenject;

namespace GameScripts.UI
{
    public class StartContinueButtons : MonoBehaviour
    {
        public float offset;
        public RectTransform continueButton;
        private IGameSaveProvider _gameSaveProvider;

        [Inject]
        public void Construct(IGameSaveProvider gameSaveProvider)
        {
            _gameSaveProvider = gameSaveProvider;
        }

        public void UpdateButtons()
        {
            if (_gameSaveProvider.HasSavedGame)
            {
                ShowContinueButton();
            }
            else
            {
                HideContinueButton();
            }
        }

        private void ShowContinueButton()
        {
            continueButton.DOAnchorPosY(-offset, 0.3f).SetEase(Ease.OutBack);
        }

        private void HideContinueButton()
        {
            continueButton.DOAnchorPosY(0, 0.3f).SetEase(Ease.OutQuad);
        }
    }
}