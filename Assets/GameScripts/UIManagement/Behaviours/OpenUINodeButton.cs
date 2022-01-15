using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UIManagement
{
    [RequireComponent(typeof(Button))]
    public class OpenUINodeButton : MonoBehaviour
    {
        [SerializeField] private UINodeId _nodeId;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.OnClickAsObservable().Subscribe(_ => UIManager.Instance.ShowViewNode(_nodeId)).AddTo(this);
        }
    }
}