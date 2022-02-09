using GameScripts.Game;

namespace GameScripts.Providers
{
    public interface IGameSaveProvider
    {
        bool HasSavedGame { get; }
        void ClearSaveData();
        void SaveGame(FieldModel fieldModel);
        FieldModel LoadSavedGame();
    }
}