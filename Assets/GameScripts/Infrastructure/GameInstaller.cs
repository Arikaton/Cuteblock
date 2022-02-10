using GameScripts.Calendar;
using GameScripts.ConsumeSystem.Module;
using GameScripts.DailyTimer;
using GameScripts.Game;
using GameScripts.PlayerStats;
using GameScripts.Providers;
using GameScripts.ResourceStorage.Interfaces;
using Zenject;

namespace GameScripts.Infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public ShapeCatalog shapeCatalog;
        public ShapeSpritesProvider shapeSpritesProvider;
        public GameStarter gameStarter;
        public WeightsCatalog weightsCatalog;

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
            BindGameSaveProvider();
            BindGameStarter();
            BindWeightsProvider();
            BindSoundAndHapticSettingsProvider();
            BindPlayerStats();
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
            Container.Bind<IShapeSpritesProvider>().To<ShapeSpritesProvider>().FromInstance(shapeSpritesProvider);
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
            Container.Bind<ReplacementHintViewModel>().FromNew().AsSingle();
            Container.Bind<DeleteHintViewModel>().FromNew().AsSingle();
        }

        private void BindGameSaveProvider()
        {
            Container.Bind<IGameSaveProvider>().To<GameSaveProvider>().AsSingle();
        }

        private void BindGameStarter()
        {
            Container.Bind<GameStarter>().FromInstance(gameStarter).AsSingle();
        }

        private void BindWeightsProvider()
        {
            Container.Bind<IWeightsProvider>().FromInstance(weightsCatalog).AsSingle();
        }

        private void BindSoundAndHapticSettingsProvider()
        {
            Container.Bind<ISoundAndHapticSettingsProvider>().To<SoundAndHapticSettingsProvider>().AsSingle();
        }

        private void BindPlayerStats()
        {
            var model = new PlayerStatsModel();
            var viewModel = new PlayerStatsViewModel(model);
            Container.Bind<PlayerStatsViewModel>().FromInstance(viewModel).AsSingle();
        }
    }
}