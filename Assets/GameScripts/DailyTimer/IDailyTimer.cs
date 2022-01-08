using System;
using UniRx;

namespace GameScripts.DailyTimer
{
    public interface IDailyTimer
    {
        int DaysAfterLastSession { get; }
        bool IsPlayedToday { get; }
        /// <summary>
        /// Represents count of days, where was at least one session
        /// </summary>
        IReadOnlyReactiveProperty<int> SessionsCount { get; }
        DateTime Today { get; }
    }
}