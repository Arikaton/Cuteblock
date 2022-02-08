using GameScripts.Game;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameScripts.UI
{
    public class StartNewGameButton : MonoBehaviour
    {
        private GameStarter _gameStarter;

        public Button button;

        [Inject]
        public void Construct(GameStarter gameStarter)
        {
            _gameStarter = gameStarter;
        }

        private void Start()
        {
            button.OnClickAsObservable().Subscribe(_ => _gameStarter.StartNewGame());
        }
    }
}