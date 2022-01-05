using UnityEngine;

namespace GameScripts.Game
{
    [CreateAssetMenu(fileName = "ShapeModelCatalog", menuName = "Game/ShapeModelCatalog", order = 0)]
    public class ShapeModelCatalog : ScriptableObject
    {
        public ShapeModel model;
    }
}