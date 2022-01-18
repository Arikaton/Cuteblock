using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.Providers
{
    [CreateAssetMenu(fileName = "ShapesCatalog", menuName = "Cuteblock/Shapes Catalog", order = 0)]
    public class ShapeSpritesCatalog : ScriptableObject, IShapeSpritesProvider
    {
        public List<Sprite> shapes = new List<Sprite>();
        
        public Sprite GetShapeSprite(int uid)
        {
            return shapes[uid];
        }
    }
}