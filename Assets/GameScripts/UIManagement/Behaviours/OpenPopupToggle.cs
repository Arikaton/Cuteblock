using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UIManagement
{
    [RequireComponent(typeof(Toggle))]
    public class OpenPopupToggle : MonoBehaviour
    {
        [SerializeField] private UIViewId _popupId;

        private void Start()
        {
            GetComponent<Toggle>().OnValueChangedAsObservable().Skip(1).Where(x => x).Subscribe(_ =>
            {
                UIManager.Instance.ShowPopup(_popupId);
            }).AddTo(this);
        }
    }
}