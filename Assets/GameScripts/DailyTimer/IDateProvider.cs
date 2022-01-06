using System;

namespace GameScripts.DailyTimer
{
    public interface IDateProvider
    {
        DateTime Today { get; }
    }
}