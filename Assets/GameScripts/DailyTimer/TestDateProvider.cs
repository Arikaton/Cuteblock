using System;

namespace GameScripts.DailyTimer
{
    public class TestDateProvider : IDateProvider
    {
        private int _additionalDays = 0;
        private LocalDailyTimer _localDailyTimer;

        public DateTime Today => DateTime.Now.AddDays(_additionalDays);

        public void AddDay()
        {
            _additionalDays++;
        }
    }
}