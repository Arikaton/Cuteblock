using System;
using GameScripts.Providers;
using Zenject;

namespace GameScripts.UI.ConditionalButtons
{
    public class HasSavedGameCondition : Condition
    {
        private IGameSaveProvider _gameSaveProvider;

        [Inject]
        public void Construct(IGameSaveProvider gameSaveProvider)
        {
            _gameSaveProvider = gameSaveProvider;
        }

        public override void Check(Action<bool> callback)
        {
            callback.Invoke(_gameSaveProvider.HasSavedGame);
        }
    }
}