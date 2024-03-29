using GameScripts.ConsumeSystem.Module;
using GameScripts.Providers;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameScripts.Game
{
    public class GameStarter : MonoBehaviour
    {
        private IShapeCatalog _shapeCatalog;
        private FieldViewModelContainer _fieldViewModelContainer;
        private AbstractConsumableFactory _consumableFactory;
        private CurrentLevelProvider _currentLevelProvider;
        private IWeightsProvider _weightsProvider;
        private LevelsProvider _levelsProvider;

        [Inject]
        public void Construct(IShapeCatalog shapeCatalog, FieldViewModelContainer fieldViewModelContainer, AbstractConsumableFactory consumableFactory,
            CurrentLevelProvider currentLevelProvider, LevelsProvider levelsProvider, IWeightsProvider weightsProvider)
        {
            _shapeCatalog = shapeCatalog;
            _fieldViewModelContainer = fieldViewModelContainer;
            _consumableFactory = consumableFactory;
            _currentLevelProvider = currentLevelProvider;
            _levelsProvider = levelsProvider;
            _weightsProvider = weightsProvider;
        }

        private void Start()
        {
            StartCurrentLevel();
        }

        public void StartCurrentLevel()
        {
            int level = _currentLevelProvider.CurrentLevel.Value;
            if (level <= 3)
            {
                StartLevelInternalOnlyOneCellCats(level);
            }
            else
            {
                StartLevelInternal(level);
            }
        }

        public void StartNextLevel()
        {
            int level = ++_currentLevelProvider.CurrentLevel.Value;
            if (level <= 3)
            {
                StartLevelInternalOnlyOneCellCats(level);
            }
            else
            {
                StartLevelInternal(level);
            }
        }

        private void StartLevelInternal(int level)
        {
            var levelData = _levelsProvider.GetLevelData(level);
            var fieldModel = new FieldModel(levelData.cellsWithGems, Random.Range(-3, 0), level);
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
            _fieldViewModelContainer.FieldViewModel.Value = fieldViewModel;
        }

        private void StartLevelInternalOnlyOneCellCats(int level)
        {
            var levelData = _levelsProvider.GetLevelData(level);
            var fieldModel = new FieldModel(levelData.cellsWithGems, Random.Range(-3, 0), level);
            var weightsCatalog = new WeightsCatalog(_weightsProvider.Weights);

            var shapeData1 = _shapeCatalog.Shapes[1];
            var shapeData2 = _shapeCatalog.Shapes[1];
            var shapeData3 = _shapeCatalog.Shapes[1];
            var availableShape0 = new ShapeModel(shapeData1.Uid, ExtensionMethods.GetRandomRotation());
            var availableShape1 = new ShapeModel(shapeData2.Uid, ExtensionMethods.GetRandomRotation());
            var availableShape2 = new ShapeModel(shapeData3.Uid, ExtensionMethods.GetRandomRotation());
            fieldModel.AvailableShapes = new ShapeModel[3] {availableShape0, availableShape1, availableShape2};
            
            var fieldViewModel = new FieldViewModel(fieldModel, _shapeCatalog, _consumableFactory, weightsCatalog);
            _fieldViewModelContainer.FieldViewModel.Value = fieldViewModel;
        }
    }
}