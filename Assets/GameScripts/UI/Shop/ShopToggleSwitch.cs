using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UI.Shop
{
    public class ShopToggleSwitch : MonoBehaviour
    {
        public Toggle gemsToggle;
        public Toggle coinsToggle;
        public Toggle hintsToggle;

        public void ActivateGemsToggle()
        {
            if (!gemsToggle.isOn) gemsToggle.isOn = true;
        }
        
        public void ActivateCoinsToggle()
        {
            if (!coinsToggle.isOn) coinsToggle.isOn = true;
        }
        
        public void ActivateHintsToggle()
        {
            if (!hintsToggle.isOn) hintsToggle.isOn = true;
        }
    }
}