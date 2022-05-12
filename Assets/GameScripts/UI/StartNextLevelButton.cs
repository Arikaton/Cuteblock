using GameScripts.Game;
using GameScripts.UIManagement;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameScripts.UI
{
    public class StartNextLevelButton : MonoBehaviour
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
            button.OnClickAsObservable().Subscribe(_ => Play()).AddTo(this);
        }
        
        private void Play()
        {
            _gameStarter.StartNextLevel();
            UIManager.Instance.ShowViewNode(UINodeId.Game);
        }
    }
}