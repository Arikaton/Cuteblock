using GameScripts.ConsumeSystem.Module;
using GameScripts.PlayerStats;
using GameScripts.Providers;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameScripts.Game
{
    public class GameStarter : MonoBehaviour
    {
        private IShapeCatalog _shapeCatalog;
        private FieldViewModelContainer _fieldViewModelContainer;
        private AbstractConsumableFactory _consumableFactory;
        private IGameSaveProvider _gameSaveProvider;
        private IWeightsProvider _weightsProvider;
        private PlayerStatsViewModel _playerStats;

        [Inject]
        public void Construct(IShapeCatalog shapeCatalog, FieldViewModelContainer fieldViewModelContainer, AbstractConsumableFactory consumableFactory,
            IGameSaveProvider gameSaveProvider, IWeightsProvider weightsProvider, PlayerStatsViewModel playerStats)
        {
            _shapeCatalog = shapeCatalog;
            _fieldViewModelContainer = fieldViewModelContainer;
            _consumableFactory = consumableFactory;
            _gameSaveProvider = gameSaveProvider;
            _weightsProvider = weightsProvider;
            _playerStats = playerStats;
        }

        public void StartSavedGame()
        {
            if (!_gameSaveProvider.HasSavedGame) return;
            var fieldModel = _gameSaveProvider.LoadSavedGame();

            var weightsCatalog = new WeightsCatalog(_weightsProvider.Weights);
            var fieldViewModel = new FieldViewModel(fieldModel, _shapeCatalog, _consumableFactory, weightsCatalog);
            fieldViewModel.OnGameFinished.Subscribe(_ => FinishGame(fieldModel)).AddTo(this);
            fieldViewModel.OnModelChanged.Subscribe(_ => SaveFieldModel(fieldModel)).AddTo(this);
            _fieldViewModelContainer.FieldViewModel.Value = fieldViewModel;
        }

        public void StartNewGame()
        {
            _gameSaveProvider.ClearSaveData();
            
            var fieldModel = new FieldModel();
            var weightsCatalog = new WeightsCatalog(_weightsProvider.Weights);

            var shapeIds = weightsCatalog.GetThreeUniqueRandomShapeId(0);
            var shapeData1 = _shapeCatalog.Shapes[shapeIds[0]];
            var shapeData2 = _shapeCatalog.Shapes[shapeIds[1]];
            var shapeData3 = _shapeCatalog.Shapes[shapeIds[2]];
            var availableShape0 = new ShapeModel(shapeData1.Uid, ExtensionMethods.GetRandomRotation());
            var availableShape1 = new ShapeModel(shapeData2.Uid, ExtensionMethods.GetRandomRotation());
            var availableShape2 = new ShapeModel(shapeData3.Uid, ExtensionMethods.GetRandomRotation());
            fieldModel.AvailableShapes = new ShapeModel[3] {availableShape0, availableShape1, availableShape2};
            
            var fieldViewModel = new FieldViewModel(fieldModel, _shapeCatalog, _consumableFactory, weightsCatalog);
            fieldViewModel.OnGameFinished.Subscribe(_ => FinishGame(fieldModel)).AddTo(this);
            fieldViewModel.OnModelChanged.Subscribe(_ => SaveFieldModel(fieldModel)).AddTo(this);
            _fieldViewModelContainer.FieldViewModel.Value = fieldViewModel;
        }

        public void FinishGame(FieldModel fieldModel)
        {
            Debug.Log("Game Finished");
            _playerStats.RecordGameScore(fieldModel.Score.Value);
            _gameSaveProvider.ClearSaveData();
        }

        private void SaveFieldModel(FieldModel fieldModel)
        {
            _gameSaveProvider.SaveGame(fieldModel);
        }
    }
}