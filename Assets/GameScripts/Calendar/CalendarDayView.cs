using System;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameScripts.Calendar
{
    [RequireComponent(typeof(Button))]
    public class CalendarDayView : MonoBehaviour
    {
        [SerializeField] private Image _cover;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _passedDayColor;
        [SerializeField] private Color _futureDayColor;

        private Button _button;
        private Sequence _sequence;
        private IReactiveProperty<CalendarDayState> _state = new ReactiveProperty<CalendarDayState>(CalendarDayState.Empty);
        private DateTime _day;
        private CalendarViewModel _viewModel;

        [Inject]
        public void Construct(CalendarViewModel viewModel)
        {
            _viewModel = viewModel;
            _button = GetComponent<Button>();
            _state.Select(state => state == CalendarDayState.Passed)
                .BindToButtonOnClick(_button, OnClick).AddTo(this);
            viewModel.SelectedDay.Subscribe(OnSelectedDayChanged).AddTo(this);

        }

        private void OnClick(Unit unit)
        {
            _viewModel.ChangeSelectedDay(_day);
        }

        private void OnSelectedDayChanged(DateTime newDay)
        {
            
            if (_state.Value == CalendarDayState.Selected)
                SetPassed(); 
            if (_day == newDay)
                SetSelected();
        }

        public void SetDay(DateTime day)
        {
            _day = day;
            _text.text = day.Day.ToString();
        }

        private void ResetState()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
        }

        public void SetEmpty()
        {
            ResetState();
            _sequence.Append(_cover.DOFade(0, 0.1f));
            _sequence.Join(_text.DOFade(0, 0.1f));
            _state.Value = CalendarDayState.Empty;
        }

        public void SetSelected()
        {
            ResetState();
            _sequence.Append(_cover.DOFade(1, 0.3f));
            _sequence.Join(_text.DOColor(_selectedColor, 0.3f));
            _state.Value = CalendarDayState.Selected;
        }

        public void SetPassed()
        {
            ResetState();
            _sequence.Append(_text.DOColor(_passedDayColor, 0.3f));
            _sequence.Join(_cover.DOFade(0, 0.3f));
            _state.Value = CalendarDayState.Passed;
        }

        public void SetFuture()
        {
            ResetState();
            _sequence.Append(_text.DOColor(_futureDayColor, 0.3f));
            _sequence.Join(_cover.DOFade(0, 0.3f));
            _state.Value = CalendarDayState.Future;
        }
    }

    public enum CalendarDayState
    {
        Selected,
        Passed,
        Future,
        Empty
    }
}