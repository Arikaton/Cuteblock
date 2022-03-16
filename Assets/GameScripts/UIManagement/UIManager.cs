using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.UIManagement
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private NodesCollections _nodesCollections;
        private Dictionary<UIViewId, UIView> _views = new Dictionary<UIViewId, UIView>();
        private Dictionary<UINodeId, ViewNode> _nodes = new Dictionary<UINodeId, ViewNode>(10);
        private HashSet<UIViewId> _activeViews = new HashSet<UIViewId>();
        private Stack<UIView> _popupStack = new Stack<UIView>();

        public UINodeId ActiveNode { get; private set; }
        public bool HasActivePopup => _popupStack.Count > 0;

        public override void AwakeInternal()
        {
            foreach (var node in _nodesCollections.Nodes)
            {
                _nodes[node.UINodeId] = node;
            }
        }

        public void Register(UIView view)
        {
            if (!_views.ContainsKey(view.ViewId))
            {
                //Debug.Log($"Register {view.ViewId} view");
                _views.Add(view.ViewId, view);
            }
        }
        
        public void ShowViewNode(UINodeId nodeId, bool hidePopups = false)
        {
            //Debug.Log($"Show {nodeId} UINode");
            if (hidePopups)
                HideAllPopups();
            
            ActiveNode = nodeId;
            var node = _nodes[nodeId];
            _activeViews.ExceptWith(node.ViewIds);
            foreach (var viewId in _activeViews)
            {
                //Debug.Log($"Hide {viewId} view");
                _views[viewId].Hide();
            }
            _activeViews.Clear();
            
            foreach (var viewId in node.ViewIds)
            {
                var view = _views[viewId];
                view.Show();
                _activeViews.Add(view.ViewId);
                //Debug.Log($"Show {view.ViewId} view");
            }
        }
        public void ShowPopup(UIViewId popupId)
        {
            var popup = _views[popupId];
            if(_popupStack.Count == 0 || _popupStack.Peek() != popup.GetUIView())
                _popupStack.Push(popup.GetUIView());
            popup.Show();
        }

        public void HideLastPopup()
        {
            if (_popupStack.Count == 0)
                return;
            var popup = _popupStack.Pop();
            popup.Hide();
        }

        public void HideAllPopups()
        {
            while (_popupStack.Count > 0)
            {
                var popup = _popupStack.Pop();
                popup.Hide();
            }
        }
    }
}