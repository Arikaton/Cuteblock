using UnityEngine;

namespace GameScripts.UIManagement
{
    public class OpenPopupAction : MonoBehaviour
    {
        [SerializeField] private UIViewId _popupId;

        public void OpenPopup()
        {
            UIManager.Instance.ShowPopup(_popupId);
        }
    }
}