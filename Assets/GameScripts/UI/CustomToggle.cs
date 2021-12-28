using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameScripts.UI
{
    [RequireComponent(typeof(Toggle))]
    public class CustomToggle : UIBehaviour
    {
        private Toggle _toggle;
        [SerializeField] private Image[] _targets;
        [SerializeField] private float _duration = 0.5f;

        protected override void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        protected override void Start()
        {
            OnToggleValueChanged(_toggle.isOn);
        }

        protected override void OnEnable()
        {
            _toggle.onValueChanged.AddListener(OnToggleValueChanged);

        }

        protected override void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(OnToggleValueChanged);

        }

        private void OnToggleValueChanged(bool isOn)
        {
            foreach (var target in _targets)
            {
                target.DOFade(isOn ? 1 : 0, _duration);
            }
        }
    }
}