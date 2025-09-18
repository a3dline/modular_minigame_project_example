using System;
using VContainer.Unity;

namespace Game.GameManager
{
    public class GameLauncherTypeReference<TGameLauncher> : ITypeReference
        where TGameLauncher : IGameLauncher
    {
        public Type Type => typeof(TGameLauncher);
    }

    public class ScopeTypeReference<TScope> : ITypeReference
        where TScope : IInstaller, new()
    {
        public Type Type => typeof(TScope);
    }
}