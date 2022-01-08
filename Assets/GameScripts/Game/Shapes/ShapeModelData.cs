using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.Game
{
    [CreateAssetMenu(fileName = "ShapeModelData", menuName = "Game/ShapeModelData", order = 0)]
    public class ShapeModelData : ScriptableObject
    {
        public List<ShapeModel> model;
    }
}