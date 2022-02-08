using GameScripts.Game;
using GameScripts.Providers;
using GameScripts.UIManagement;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameScripts.UI
{
    public class PlayButton : MonoBehaviour
    {
        public Button button;
        private IGameSaveProvider _gameSaveProvider;
        private GameStarter _gameStarter;

        [Inject]
        public void Construct(IGameSaveProvider gameSaveProvider, GameStarter gameStarter)
        {
            _gameSaveProvider = gameSaveProvider;
            _gameStarter = gameStarter;
        }

        private void Start()
        {
            button.OnClickAsObservable().Subscribe(_ => Play());
        }

        private void Play()
        {
            if (_gameSaveProvider.HasSavedGame)
            {
                UIManager.Instance.ShowPopup(UIViewId.PopupContinueOrNewGame);
                return;
            }

            _gameStarter.StartNewGame();
        }
    }
}