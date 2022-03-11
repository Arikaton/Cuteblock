using UnityEngine;

namespace GameScripts.UIManagement
{
    public class OpenUINodeAction : MonoBehaviour
    {
        [SerializeField] private UINodeId _nodeId;

        public void Invoke()
        {
            UIManager.Instance.ShowViewNode(_nodeId);
        }
    }
}