using GameScripts.Providers;
using UnityEngine;
using Zenject;

namespace GameScripts.UI
{
    public class ShapeViewFactory : MonoBehaviour
    {
        [SerializeField] private ShapeView shapeViewPrefab;
        [SerializeField] private RectTransform fieldRect;
        [SerializeField] private RectTransform[] availableFigureContainers = new RectTransform[3];
        [SerializeField] private FieldView fieldView;
        private IShapeSpritesProvider _shapeSpritesProvider;

        [Inject]
        public void Construct(IShapeSpritesProvider shapeSpritesProvider)
        {
            _shapeSpritesProvider = shapeSpritesProvider;
        }
        
        public ShapeView CreateShapeView(int shapeIndex)
        {
            var shapeView = Instantiate(shapeViewPrefab, availableFigureContainers[shapeIndex]);
            shapeView.Initialize(fieldRect, _shapeSpritesProvider, shapeIndex, fieldView);
            return shapeView;
        }
    }
}