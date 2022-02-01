using GameScripts.Calendar;
using GameScripts.ConsumeSystem.Module;
using GameScripts.DailyTimer;
using GameScripts.Game;
using GameScripts.Providers;
using GameScripts.ResourceStorage.Interfaces;
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
            BindResourceStorage();
            BindConsumableFactory();
            BindHints();
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

        private void BindResourceStorage()
        {
            Container.Bind<IResourceStorage>().FromInstance(ResourceStorageFactory.CreateResourceStorage()).AsSingle();
        }

        private void BindConsumableFactory()
        {
            Container.Bind<AbstractConsumableFactory>().To<ConsumableFactory>().FromNew().AsSingle();
        }

        private void BindHints()
        {
            Container.Bind<RotateHintViewModel>().FromNew().AsSingle();
            Container.Bind<NewShapesHintViewModel>().FromNew().AsSingle();
            Container.Bind<DeleteHintViewModel>().FromNew().AsSingle();
        }
    }
}