using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UIManagement
{
    [RequireComponent(typeof(Button))]
    public class OpenPopupButton : MonoBehaviour
    {
        [SerializeField] private UIViewId _popupId;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.OnClickAsObservable().Subscribe(_ => UIManager.Instance.ShowPopup(_popupId)).AddTo(this);
        }
    }
}