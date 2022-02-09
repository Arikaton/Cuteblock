using GameScripts.ConsumeSystem.Module;
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

        [Inject]
        public void Construct(IShapeCatalog shapeCatalog, FieldViewModelContainer fieldViewModelContainer, AbstractConsumableFactory consumableFactory,
            IGameSaveProvider gameSaveProvider, IWeightsProvider weightsProvider)
        {
            _shapeCatalog = shapeCatalog;
            _fieldViewModelContainer = fieldViewModelContainer;
            _consumableFactory = consumableFactory;
            _gameSaveProvider = gameSaveProvider;
            _weightsProvider = weightsProvider;
        }

        public void StartSavedGame()
        {
            var fieldModel = _gameSaveProvider.LoadSavedGame();
            
            var fieldViewModel = new FieldViewModel(fieldModel, _shapeCatalog, _consumableFactory, _weightsProvider.Weights);
            fieldViewModel.OnGameFinished.Subscribe(_ => FinishGame()).AddTo(this);
            fieldViewModel.OnModelChanged.Subscribe(_ => SaveFieldModel(fieldModel)).AddTo(this);
            _fieldViewModelContainer.FieldViewModel.Value = fieldViewModel;
        }

        public void StartNewGame()
        {
            var fieldModel = new FieldModel();
                
            var shapeData1 = _shapeCatalog.Shapes[_weightsProvider.Weights.GetRandomWeightedIndex()];
            var shapeData2 = _shapeCatalog.Shapes[_weightsProvider.Weights.GetRandomWeightedIndex()];
            var shapeData3 = _shapeCatalog.Shapes[_weightsProvider.Weights.GetRandomWeightedIndex()];
            var availableShape0 = new ShapeModel(shapeData1.Uid, ExtensionMethods.GetRandomRotation());
            var availableShape1 = new ShapeModel(shapeData2.Uid, ExtensionMethods.GetRandomRotation());
            var availableShape2 = new ShapeModel(shapeData3.Uid, ExtensionMethods.GetRandomRotation());
            fieldModel.AvailableShapes = new ShapeModel[3] {availableShape0, availableShape1, availableShape2};
            
            var fieldViewModel = new FieldViewModel(fieldModel, _shapeCatalog, _consumableFactory, _weightsProvider.Weights);
            fieldViewModel.OnGameFinished.Subscribe(_ => FinishGame()).AddTo(this);
            fieldViewModel.OnModelChanged.Subscribe(_ => SaveFieldModel(fieldModel)).AddTo(this);
            _fieldViewModelContainer.FieldViewModel.Value = fieldViewModel;
        }

        public void FinishGame()
        {
            Debug.Log("Game Finished");
            _gameSaveProvider.ClearSaveData();
        }

        private void SaveFieldModel(FieldModel fieldModel)
        {
            _gameSaveProvider.SaveGame(fieldModel);
        }
    }
}