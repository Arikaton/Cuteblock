using UnityEngine;

namespace GameScripts.Providers
{
    public interface IShapeSpritesProvider
    {
        public Sprite GetShapeSprite(int uid);
    }
}