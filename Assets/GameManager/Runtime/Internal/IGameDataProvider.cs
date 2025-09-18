using System.Collections.Generic;

namespace Game.GameManager
{
    internal class GameData
    {
        public IGameManifest Manifest;
        public string SceneName;
        public string GameName;
        public string BundleName;
    }
    
    internal interface IGameDataProvider
    {
        IEnumerable<GameData> GetAllGamesData();
    }
}