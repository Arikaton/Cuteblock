using System.Globalization;
using GameScripts.DailyTimer;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameScripts.Calendar
{
    public class CalendarView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _monthYearText;
        [SerializeField] private Button _nextMonthButton;
        [SerializeField] private Button _prevMonthButton;
        [SerializeField] private CalendarDayView[] _calendarDayViews;
        private IDateProvider _dateProvider;
        private CalendarViewModel _viewModel;

        [Inject]
        public void Construct(CalendarViewModel viewModel, IDateProvider dateProvider)
        {
            _viewModel = viewModel;
            _dateProvider = dateProvider;
            viewModel.CurrentMonthDifference.Subscribe(OnCurrentMonthChanged).AddTo(this);
            viewModel.CanTurnNext.BindToButtonOnClick(_nextMonthButton, _ => viewModel.AddMonth()).AddTo(this);
            viewModel.CanTurnPrev.BindToButtonOnClick(_prevMonthButton, _ => viewModel.SubtractMonth()).AddTo(this);
        }

        private void OnCurrentMonthChanged(int monthDifference)
        {
            UpdateMonthYearText(monthDifference);
            UpdateDays(monthDifference);
        }

        private void UpdateMonthYearText(int monthDifference)
        {
            var date = _dateProvider.Today.AddMonths(-monthDifference);
            _monthYearText.text = date.ToString("MMMM yyyy", CultureInfo.InvariantCulture);
        }

        public void UpdateDays(int monthDifference)
        {
            var currentDay = _dateProvider.Today.Date;

            var firstDayInMonth = currentDay
                .AddDays(-(currentDay.Day - 1))
                .AddMonths(-monthDifference);
            
            var firstDayOfWeek = firstDayInMonth.DayOfWeek;
            var emptyDaysCount = ((int) firstDayOfWeek + 6) % 7;
            
            for (var i = 0; i < _calendarDayViews.Length; i++)
            {
                var currentDayView = _calendarDayViews[i];
                if (i < emptyDaysCount)
                {
                    currentDayView.SetEmpty();
                    continue;
                }

                var day = firstDayInMonth.AddDays(i - emptyDaysCount);
                if (day.Month != firstDayInMonth.Month)
                {
                    currentDayView.SetEmpty();
                    continue;
                }
                
                currentDayView.SetDay(day);
                var daysDifference = Mathf.FloorToInt((float)currentDay.Subtract(day).TotalDays);
                if (daysDifference >= 0)
                {
                    if (_viewModel.SelectedDay.Value == day)
                    {
                        currentDayView.SetSelected();
                    }
                    else
                    {
                        currentDayView.SetPassed();
                    }
                } 
                else
                {
                    currentDayView.SetFuture();
                }
            }
        }
    }
}