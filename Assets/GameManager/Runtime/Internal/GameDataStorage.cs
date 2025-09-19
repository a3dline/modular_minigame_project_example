using UnityEngine;

namespace Game.GameManager
{
    public class GameDataStorage : ScriptableObject
    {
        public const string FileName = "GameData";
        public GameData[] Data;
    }
}