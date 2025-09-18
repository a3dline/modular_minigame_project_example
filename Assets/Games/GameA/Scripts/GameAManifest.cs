using Game.Common;
using Game.GameManager;

namespace Games.GameA
{
    public class GameAManifest : IGameManifest
    {
        public string Name => "GameA";
        public ITypeReference LauncherType => new GameLauncherTypeReference<GameALauncher>();
        public ITypeReference ScopeType => new ScopeTypeReference<GameAScope>();
    }
}