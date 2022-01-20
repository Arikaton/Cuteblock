using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameScripts.Game
{
    public class GameStarter : MonoBehaviour
    {
        private IShapeCatalog _shapeCatalog;
        private FieldViewModelContainer _fieldViewModelContainer;
        private CompositeDisposable _disposables;

        private void Awake()
        {
            _disposables = new CompositeDisposable();
        }

        private void Start()
        {
            StartGame();
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        [Inject]
        public void Construct(IShapeCatalog shapeCatalog, FieldViewModelContainer fieldViewModelContainer)
        {
            _shapeCatalog = shapeCatalog;
            _fieldViewModelContainer = fieldViewModelContainer;
        }

        public void StartGame()
        {
            var fieldModel = new FieldModel();
            var shapeData1 = _shapeCatalog.Shapes[1];
            var shapeData2 = _shapeCatalog.Shapes[2];
            var availableShape0 = new ShapeModel(shapeData1.Uid, Rotation.Deg90);
            var availableShape1 = new ShapeModel(shapeData2.Uid, Rotation.Deg90);
            var availableShape2 = new ShapeModel(shapeData1.Uid, Rotation.Deg90);
            fieldModel.AvailableShapes = new ShapeModel[3] {availableShape0, availableShape1, availableShape2};
            
            var fieldViewModel = new FieldViewModel(fieldModel, _shapeCatalog);
            fieldViewModel.OnGameFinished.Subscribe(_ => FinishGame()).AddTo(_disposables);
            _fieldViewModelContainer.FieldViewModel.Value = fieldViewModel;
            
            Debug.Log("Game Started");
        }

        public void FinishGame()
        {
            Debug.Log("Game Finished");
        }
    }
}