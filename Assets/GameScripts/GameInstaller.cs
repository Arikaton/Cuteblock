using GameScripts.Calendar;
using GameScripts.DailyTimer;
using Zenject;

namespace GameScripts
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindDailyTimer();
            Container.Bind<CalendarViewModel>().FromNew().AsSingle();
        }

        private void BindDailyTimer()
        {
            #if DEBUG
            Container.Bind<IDateProvider>().To<TestDateProvider>().FromNew().AsSingle();
            #else
            Container.Bind<IDateProvider>().To<LocalDateProvider>().FromNew().AsSingle();
            #endif
            Container.Bind<IDailyTimer>().To<LocalDailyTimer>().FromNew().AsSingle();
        }
    }
}