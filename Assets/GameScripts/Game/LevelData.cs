using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.Game
{
    [System.Serializable]
    public class LevelData
    {
        public List<Vector2Int> cellsWithGems = new List<Vector2Int>();
    }
}