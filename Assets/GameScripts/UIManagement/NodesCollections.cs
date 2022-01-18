using UnityEngine;

namespace GameScripts.UIManagement
{
    [CreateAssetMenu(fileName = "UINodes collections", menuName = "New UINodes Collection", order = 0)]
    public class NodesCollections : ScriptableObject
    {
        public ViewNode[] Nodes;
    }
}