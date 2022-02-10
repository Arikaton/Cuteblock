using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScripts.UI
{
    public class SetAsLastSiblingOnPointerDown : MonoBehaviour, IPointerDownHandler
    {

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.SetAsLastSibling();
        }
    }
}