namespace GameScripts.Game
{
    public class GameStarter
    {
        private IShapeCatalog _shapeCatalog;
        private FieldViewModelContainer _fieldViewModelContainer;
        private ShapeViewModelsContainer _shapeViewModelsContainer;

        public GameStarter(IShapeCatalog shapeCatalog, FieldViewModelContainer fieldViewModelContainer, ShapeViewModelsContainer shapeViewModelsContainer)
        {
            _shapeCatalog = shapeCatalog;
            _fieldViewModelContainer = fieldViewModelContainer;
            _shapeViewModelsContainer = shapeViewModelsContainer;
        }

        public void StartGame()
        {
            var fieldModel = new FieldModel();
            var fieldViewModel = new FieldViewModel(fieldModel, _shapeCatalog);
            _fieldViewModelContainer.FieldViewModel.Value = fieldViewModel;

            var shapeData = _shapeCatalog.Shapes[0];// Заменить на получение рандомной фигуры
            var firstShapeModel = new ShapeModel(shapeData.uid, Rotation.Deg0);
            var secondShapeModel = new ShapeModel(shapeData.uid, Rotation.Deg0);
            var thirdShapeModel = new ShapeModel(shapeData.uid, Rotation.Deg0);

            var firstShapeViewModel = new ShapeViewModel(firstShapeModel);
            var secondShapeViewModel = new ShapeViewModel(secondShapeModel);
            var thirdShapeViewModel = new ShapeViewModel(thirdShapeModel);
            _shapeViewModelsContainer.ChangeViewModel(0, firstShapeViewModel);
            _shapeViewModelsContainer.ChangeViewModel(0, secondShapeViewModel);
            _shapeViewModelsContainer.ChangeViewModel(0, thirdShapeViewModel);
        }
    }
}