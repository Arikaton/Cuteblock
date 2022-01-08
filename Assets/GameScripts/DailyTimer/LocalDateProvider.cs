using System;

namespace GameScripts.DailyTimer
{
    public class LocalDateProvider : IDateProvider
    {
        public DateTime Today => DateTime.Today;
    }
}