using Game.GameManager;

namespace MyGame
{
    public class MyGameManifest : IGameManifest
    {
        public ITypeReference LauncherType { get; }
        public ITypeReference ScopeType { get; }
    }
}