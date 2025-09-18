using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameManager.Editor;
using UnityEditor;

namespace Game.GameManager
{
    internal class EditorGameDataProvider : IGameDataProvider
    {
        public IEnumerable<GameData> GetAllGamesData()
        {
            var foldersEnumerator = AssetDatabase.FindAssets("t:DefaultAsset").Select(AssetDatabase.GUIDToAssetPath);
            foreach (var folder in foldersEnumerator)
            {
                if (GameMetadataStorage.IsGameEnabled(folder))
                {
                    var data = new GameData();
                    
                    var manifest = GameMetadataStorage.GetManifest(folder);
                    if (manifest != null)
                    {
                        data.Manifest = Activator.CreateInstance(manifest.GetClass()) as IGameManifest;
                        data.SceneName = GameMetadataStorage.GetScene(folder)?.name;
                        data.GameName = GameMetadataStorage.GetGameName(folder);
                    }

                    yield return data;
                }
            }
        }
    }
}