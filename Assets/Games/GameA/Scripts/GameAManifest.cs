using Game.GameManager;

namespace Games.GameA
{
    public class GameAManifest : IGameManifest
    {
        public ITypeReference LauncherType => new GameLauncherTypeReference<GameALauncher>();
        public ITypeReference ScopeType => new ScopeTypeReference<GameAScope>();
    }
}