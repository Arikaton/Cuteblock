using UnityEngine;

namespace GameScripts.UIManagement
{
    [CreateAssetMenu(fileName = "New View Node", menuName = "Create new view node")]
    public class ViewNode : ScriptableObject
    {
        public UINodeId UINodeId;
        public UIViewId[] ViewIds;
    }

    public enum UINodeId
    {
        MainMenu,
        Game,
        Shop,
        Daily,
        Calendar
    }
}