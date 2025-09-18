using System;

namespace Game.GameManager
{
    public interface ITypeReference
    {
        Type Type { get; }
    }

    public interface IGameManifest
    {
        string Name { get; }
        ITypeReference LauncherType { get; }
        ITypeReference ScopeType { get; }
    }
}