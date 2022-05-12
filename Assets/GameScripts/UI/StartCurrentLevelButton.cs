using GameScripts.Game;
using GameScripts.UIManagement;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameScripts.UI
{
    public class StartCurrentLevelButton : MonoBehaviour
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
            _gameStarter.StartCurrentLevel();
            UIManager.Instance.ShowViewNode(UINodeId.Game);
        }
    }
}