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

        private void Start()
        {
            StartGame();
        }

        [Inject]
        public void Construct(IShapeCatalog shapeCatalog, FieldViewModelContainer fieldViewModelContainer)
        {
            _shapeCatalog = shapeCatalog;
            _fieldViewModelContainer = fieldViewModelContainer;
        }

        [DisableInEditorMode, Button]
        public void StartGame()
        {
            var fieldModel = new FieldModel();
            var shapeData1 = _shapeCatalog.Shapes[1];
            var shapeData2 = _shapeCatalog.Shapes[2];
            var availableShape0 = new ShapeModel(shapeData1.Uid, Rotation.Deg0);
            var availableShape1 = new ShapeModel(shapeData2.Uid, Rotation.Deg0);
            var availableShape2 = new ShapeModel(shapeData1.Uid, Rotation.Deg0);
            fieldModel.AvailableShapes = new ShapeModel[3] {availableShape0, availableShape1, availableShape2};
            
            var fieldViewModel = new FieldViewModel(fieldModel, _shapeCatalog);
            _fieldViewModelContainer.FieldViewModel.Value = fieldViewModel;
            
            Debug.Log("Game Started");
        }
    }
}