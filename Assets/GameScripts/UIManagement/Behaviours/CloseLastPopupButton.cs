using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UIManagement
{
    [RequireComponent(typeof(Button))]
    public class CloseLastPopupButton : MonoBehaviour
    {
        private void Start()
        {
            var button = GetComponent<Button>();
            button.OnClickAsObservable().Subscribe(_ => UIManager.Instance.HideLastPopup());
        }
    }
}