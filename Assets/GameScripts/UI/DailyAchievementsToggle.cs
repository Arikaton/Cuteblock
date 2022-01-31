using System;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameScripts.UI
{
    public class DailyAchievementsToggle : MonoBehaviour
    {
        [SerializeField] private RectTransform _toggleRectTransform;
        [SerializeField] private Ease _ease;
        [SerializeField] private float _duration;
        
        private bool _isDaily = true;
        private float _startPosX;
        private Image _image;

        private void Start()
        {
            _startPosX = _toggleRectTransform.anchoredPosition.x;
            _image = GetComponent<Image>();
            _image.OnPointerClickAsObservable().Subscribe(ChangeState).AddTo(this);
        }

        private void ChangeState(PointerEventData _)
        {
            _toggleRectTransform.DOAnchorPosX(_isDaily ? -_startPosX : _startPosX, _duration).SetEase(_ease);
            _isDaily = !_isDaily;
        }
    }
}