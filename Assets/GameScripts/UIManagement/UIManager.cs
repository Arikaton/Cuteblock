using System.Collections.Generic;

namespace GameScripts.UIManagement
{
    public class UIManager : Singleton<UIManager>
    {
        private Dictionary<UIViewId, List<UIView>> _views = new();

        public void Register(UIView view)
        {
            if (_views.ContainsKey(view.ViewId))
            {
                _views[view.ViewId].Add(view);
            }
        }
        
        public void ShowViewNode(UINodeId nodeId)
        {
            
        }
        public void ShowPopup()
        {
            
        }

        public void HidePopup()
        {
            
        }

        public void HideAllPopups()
        {
            
        }
    }
}