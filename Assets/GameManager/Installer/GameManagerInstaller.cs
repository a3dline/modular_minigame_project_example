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
            
        #if UNITY_EDITOR
            builder.Register<IGameManifestsProvider, EditorGameManifestProvider>(Lifetime.Singleton);
        #else
        #endif
        }
    }
}