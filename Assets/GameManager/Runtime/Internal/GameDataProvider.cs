using System.Collections.Generic;
using UnityEngine;

namespace Game.GameManager
{
    public class GameDataProvider : IGameDataProvider
    {
        public IEnumerable<GameData> GetAllGamesData()
        {
            var gameData = Resources.Load<GameDataStorage>(GameDataStorage.FileName);
            return gameData.Data;
        }
    }
}