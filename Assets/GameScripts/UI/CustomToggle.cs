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
        [SerializeField] private Image _cover;
        [SerializeField] private Image _icon;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Color _selectedColor;

        private Color _startColor;

        protected override void Awake()
        {
            _startColor = _icon.color;
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
            _cover.DOFade(isOn ? 1 : 0, _duration);
            _icon.DOColor(isOn ? _selectedColor : _startColor, _duration);
        }
    }
}