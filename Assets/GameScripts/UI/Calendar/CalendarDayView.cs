using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts.UI
{
    public class CalendarDayView : MonoBehaviour
    {
        [SerializeField] private Image _cover;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _passedDayColor;
        [SerializeField] private Color _futureDayColor;

        public void SetDay(int day)
        {
            _text.text = day.ToString();
        }

        public void SetSelected()
        {
            _cover.DOFade(1, 0.3f);
            _text.color = _selectedColor;
        }

        public void SetPassed()
        {
            _cover.DOFade(0, 0.3f);
            _text.color = _passedDayColor;
        }

        public void SetFuture()
        {
            _cover.DOFade(0, 0.3f);
            _text.color = _futureDayColor;
        }
    }
}