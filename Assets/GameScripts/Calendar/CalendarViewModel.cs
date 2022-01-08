using System;
using GameScripts.DailyTimer;
using UniRx;

namespace GameScripts.Calendar
{
    public class CalendarViewModel
    {
        public const int MAX_MONTH_DIFFERENCE = 3;
        
        public readonly IReadOnlyReactiveProperty<int> CurrentMonthDifference;
        public IReadOnlyReactiveProperty<DateTime> SelectedDay;
        public readonly IReadOnlyReactiveProperty<bool> CanTurnNext;
        public readonly IReadOnlyReactiveProperty<bool> CanTurnPrev;

        private readonly IReactiveProperty<int> _currentMonthDifference;
        private readonly IReactiveProperty<DateTime> _selectedDay;

        public CalendarViewModel(IDateProvider dateProvider)
        {
            _selectedDay = new ReactiveProperty<DateTime>(dateProvider.Today.Date);
            _currentMonthDifference = new ReactiveProperty<int>(0);
            SelectedDay = _selectedDay;
            CurrentMonthDifference = _currentMonthDifference;
            CanTurnNext = CurrentMonthDifference.Select(month => month > 0).ToReactiveProperty();
            CanTurnPrev = CurrentMonthDifference.Select(month => month < MAX_MONTH_DIFFERENCE).ToReactiveProperty();
        }

        public void AddMonth()
        {
            if (!CanTurnNext.Value)
                return;
                
            _currentMonthDifference.Value--;
        }

        public void ChangeSelectedDay(DateTime date)
        {
            _selectedDay.Value = date;
        }

        public void SubtractMonth()
        {
            if (!CanTurnPrev.Value)
                return;
            _currentMonthDifference.Value++;
        }
        
    }
}