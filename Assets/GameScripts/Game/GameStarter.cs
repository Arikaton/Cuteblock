using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace GameScripts.Game
{
    public class GameStarter : MonoBehaviour
    {
        private IShapeCatalog _shapeCatalog;
        private FieldViewModelContainer _fieldViewModelContainer;
        private ShapeViewModelsContainer _shapeViewModelsContainer;

        private void Start()
        {
            StartGame();
        }

        [Inject]
        public void Construct(IShapeCatalog shapeCatalog, FieldViewModelContainer fieldViewModelContainer, ShapeViewModelsContainer shapeViewModelsContainer)
        {
            _shapeCatalog = shapeCatalog;
            _fieldViewModelContainer = fieldViewModelContainer;
            _shapeViewModelsContainer = shapeViewModelsContainer;
        }

        [DisableInEditorMode, Button]
        public void StartGame()
        {
            Debug.Log("Game Started");
            var fieldModel = new FieldModel();
            var fieldViewModel = new FieldViewModel(fieldModel, _shapeCatalog);
            _fieldViewModelContainer.FieldViewModel.Value = fieldViewModel;

            var shapeData = _shapeCatalog.Shapes[0];// TODO: Заменить на получение рандомной фигуры
            var firstShapeModel = new ShapeModel(shapeData.Uid, Rotation.Deg0);
            var secondShapeModel = new ShapeModel(shapeData.Uid, Rotation.Deg0);
            var thirdShapeModel = new ShapeModel(shapeData.Uid, Rotation.Deg0);

            var firstShapeViewModel = new ShapeViewModel(firstShapeModel);
            var secondShapeViewModel = new ShapeViewModel(secondShapeModel);
            var thirdShapeViewModel = new ShapeViewModel(thirdShapeModel);
            _shapeViewModelsContainer.ChangeViewModel(0, firstShapeViewModel);
            _shapeViewModelsContainer.ChangeViewModel(1, secondShapeViewModel);
            _shapeViewModelsContainer.ChangeViewModel(2, thirdShapeViewModel);
        }
    }
}