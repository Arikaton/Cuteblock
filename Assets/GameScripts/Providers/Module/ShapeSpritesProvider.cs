using UnityEngine;

namespace GameScripts.Providers
{
    public class ShapeSpritesProvider : MonoBehaviour, IShapeSpritesProvider
    {
        public ShapeSpritesCatalog cats1;
        public ShapeSpritesCatalog cats2;

        private string _assetPackId = "cats_1";

        public Sprite GetShapeSprite(int uid)
        {
            if (_assetPackId == "cats_1")
                return cats1.shapes[uid];
            if (_assetPackId == "cats_2")
                return cats2.shapes[uid];
            return cats1.shapes[uid];
        }
    }
}