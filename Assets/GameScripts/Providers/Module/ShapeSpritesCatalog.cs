using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.Providers
{
    [CreateAssetMenu(fileName = "ShapesCatalog", menuName = "Cuteblock/Shapes Catalog", order = 0)]
    public class ShapeSpritesCatalog : ScriptableObject, IShapeSpritesProvider
    {
        [SerializeField] private List<ShapeSprite> shapes = new List<ShapeSprite>();
        
        public Sprite GetShapeSprite(int uid)
        {
            return shapes.Find(x => x.id == uid).sprite;
        }

        [System.Serializable]
        public class ShapeSprite
        {
            public int id;
            public Sprite sprite;
        }
    }
}