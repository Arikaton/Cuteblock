using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UI.ConditionalButtons.Actions
{
    public class EnableButtonAction : MonoBehaviour
    {
        [SerializeField] private Button button;

        public void EnableButton()
        {
            button.interactable = true;
        }
    }
}