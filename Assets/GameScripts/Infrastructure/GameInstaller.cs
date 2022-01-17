using GameScripts.Calendar;
using GameScripts.DailyTimer;
using GameScripts.Game;
using GameScripts.Providers;
using GameScripts.UI;
using Zenject;

namespace GameScripts.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public ShapeCatalog shapeCatalog;
        public ShapeSpritesCatalog shapeSpritesCatalog;

        public override void InstallBindings()
        {
            BindDailyTimer();
            Container.Bind<CalendarViewModel>().FromNew().AsSingle();
            BindShapeCatalog();
            BindFieldViewModelContainer();
            BindShapeSpritesCatalog();
            BindShapeViewFactory();
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

        private void BindShapeSpritesCatalog()
        {
            Container.Bind<IShapeSpritesProvider>().To<ShapeSpritesCatalog>().FromInstance(shapeSpritesCatalog);
        }

        private void BindShapeViewFactory()
        {
            Container.Bind<ShapeViewFactory>().FromComponentInHierarchy().AsSingle();
        }
    }
}