using GameScripts.Calendar;
using GameScripts.DailyTimer;
using GameScripts.Game;
using Zenject;

namespace GameScripts.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public ShapeCatalog shapeCatalog;
        
        public override void InstallBindings()
        {
            BindDailyTimer();
            Container.Bind<CalendarViewModel>().FromNew().AsSingle();
            BindShapeCatalog();
            BindFieldViewModelContainer();
            BindShapeViewModelsContainer();
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

        private void BindShapeCatalog()
        {
            Container.Bind<IShapeCatalog>().To<ShapeCatalog>().FromInstance(shapeCatalog).AsSingle();
        }

        private void BindFieldViewModelContainer()
        {
            Container.Bind<FieldViewModelContainer>().FromNew().AsSingle();
        }

        private void BindShapeViewModelsContainer()
        {
            Container.Bind<ShapeViewModelsContainer>().FromNew().AsSingle();
        }
    }
}