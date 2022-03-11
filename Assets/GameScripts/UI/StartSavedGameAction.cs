using GameScripts.Game;
using UnityEngine;
using Zenject;

namespace GameScripts.UI
{
    public class StartSavedGameAction : MonoBehaviour
    {
        private GameStarter _gameStarter;

        [Inject]
        public void Construct(GameStarter gameStarter)
        {
            _gameStarter = gameStarter;
        }

        public void Invoke()
        {
            _gameStarter.StartSavedGame();
        }
    }
}