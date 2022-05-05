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
        private GameStarter _gameStarter;

        [Inject]
        public void Construct(GameStarter gameStarter)
        {
            _gameStarter = gameStarter;
        }

        private void Start()
        {
            button.OnClickAsObservable().Subscribe(_ => Play());
        }

        private void Play()
        {
            _gameStarter.StartNewGame();
            UIManager.Instance.ShowViewNode(UINodeId.Game);
        }
    }
}