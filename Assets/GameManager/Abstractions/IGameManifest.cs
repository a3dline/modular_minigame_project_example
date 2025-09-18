namespace Game.GameManager
{
    public interface IGameManifest
    {
        ITypeReference LauncherType { get; }
        ITypeReference ScopeType { get; }
    }
}