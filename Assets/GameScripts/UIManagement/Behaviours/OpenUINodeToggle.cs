using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UIManagement
{
    [RequireComponent(typeof(Toggle))]
    public class OpenUINodeToggle : MonoBehaviour
    {
        [SerializeField] private UINodeId _nodeId;

        private void Start()
        {
            var toggle = GetComponent<Toggle>();
            toggle.OnValueChangedAsObservable().Subscribe(value =>
            {
                if (value)
                {
                    UIManager.Instance.ShowViewNode(_nodeId);
                }
            }).AddTo(this);
        }
    }
}