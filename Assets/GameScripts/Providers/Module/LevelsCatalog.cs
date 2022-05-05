using System.Collections.Generic;
using GameScripts.Game;
using UnityEngine;

namespace GameScripts.Providers
{
    [CreateAssetMenu(fileName = "LevelsCatalog", menuName = "Cuteblock/Levels Catalog", order = 0)]
    public class LevelsCatalog : ScriptableObject
    {
        public List<LevelData> levels;
    }
}