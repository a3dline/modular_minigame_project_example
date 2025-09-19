using Game.GameManager;
using UnityEngine.Scripting;

namespace Games.GameA
{
    [Preserve]
    public class GameAManifest : IGameManifest
    {
        public ITypeReference LauncherType => new GameLauncherTypeReference<GameALauncher>();
        public ITypeReference ScopeType => new ScopeTypeReference<GameAScope>();
    }
}