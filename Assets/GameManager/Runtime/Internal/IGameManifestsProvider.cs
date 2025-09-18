using System.Collections.Generic;

namespace Game.GameManager
{
    internal interface IGameManifestsProvider
    {
        IEnumerable<IGameManifest> GetGameManifests();
    }
}