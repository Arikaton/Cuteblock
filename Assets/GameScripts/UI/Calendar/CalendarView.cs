using System;
using UnityEngine;

namespace GameScripts.UI.Calendar
{
    public class CalendarView : MonoBehaviour
    {
        [SerializeField] private CalendarDayView _calendarDayViewPrefab;
        [SerializeField] private Transform _gridContainer;

        private void Start()
        {
            UpdateDays();
        }

        public void UpdateDays()
        {
            var currentDay = DateTime.Now;
            var firstDayInMonth = currentDay.AddDays(-(currentDay.Day - 1));
            var firstDayOfWeek = firstDayInMonth.DayOfWeek;
            var emptyDaysCount = ((int) firstDayOfWeek + 6) % 7;
            for (var i = 0; i < emptyDaysCount; i++)
            {
                var emptyDay = new GameObject("Empty Day", typeof(RectTransform));
                emptyDay.transform.SetParent(_gridContainer);
            }

            var counter = 0;
            while (firstDayInMonth.AddDays(counter).Month == currentDay.Month)
            {
                var day = firstDayInMonth.AddDays(counter++);
                var dayView = Instantiate(_calendarDayViewPrefab, _gridContainer);
                dayView.transform.localScale = Vector3.one;
                dayView.SetDay(counter);
                if (day.DayOfYear < currentDay.DayOfYear)
                {
                    dayView.SetPassed();
                } else if (day.DayOfYear == currentDay.DayOfYear)
                {
                    dayView.SetSelected();
                }
                else
                {
                    dayView.SetFuture();
                }
            }
        }
    }
}