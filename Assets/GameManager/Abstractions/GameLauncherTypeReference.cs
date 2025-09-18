using System;

namespace Game.GameManager
{
    public class GameLauncherTypeReference<TGameLauncher> : ITypeReference
        where TGameLauncher : IGameLauncher
    {
        public Type Type => typeof(TGameLauncher);
    }
}