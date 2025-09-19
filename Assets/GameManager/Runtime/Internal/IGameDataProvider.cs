using System.Collections.Generic;

namespace Game.GameManager
{
    internal interface IGameDataProvider
    {
        IEnumerable<GameData> GetAllGamesData();
    }
}