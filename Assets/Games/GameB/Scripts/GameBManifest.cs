using Game.GameManager;
using UnityEngine.Scripting;

namespace Games.GameB
{
    [Preserve]
    public class GameBManifest : IGameManifest
    {
        public ITypeReference LauncherType => new GameLauncherTypeReference<GameBLauncher>();
        public ITypeReference ScopeType => new ScopeTypeReference<GameBScope>();
    }
}