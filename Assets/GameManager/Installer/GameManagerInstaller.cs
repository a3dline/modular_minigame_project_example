using VContainer;

namespace Game.GameManager
{
    public static class GameManagerInstaller
    {
        public static void InstallDefault(IContainerBuilder builder)
        {
            builder.Register<IGameManagerController, GameManagerController>(Lifetime.Transient);
            builder.Register<GameControllerRunner>(Lifetime.Transient);
            builder.Register<SelectGamePopupViewController>(Lifetime.Transient);
            
            builder.Register<GameAssetsProviderFactory>(Lifetime.Singleton);
            builder.Register<GameSceneProviderFactory>(Lifetime.Singleton);

        #if UNITY_EDITOR
            builder.Register<IGameDataProvider, EditorGameDataProvider>(Lifetime.Singleton);
            builder.Register<IBundleProvider, EditorBundleProvider>(Lifetime.Singleton);
            builder.Register<ISceneLoader, EditorSceneLoader>(Lifetime.Singleton);
        #else
            builder.Register<IGameDataProvider, GameDataProvider>(Lifetime.Singleton);
            builder.Register<IBundleProvider, RuntimeBundleProvider>(Lifetime.Singleton);
            builder.Register<ISceneLoader, RuntimeSceneLoader>(Lifetime.Singleton);
        #endif
        }
    }
}