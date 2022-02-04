using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UI.ConditionalButtons.Actions
{
    public class DisableButtonAction : MonoBehaviour
    {
        [SerializeField] private Button button;

        public void DisableButton()
        {
            button.interactable = false;
        }
    }
}