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
                        data.ManifestType = manifest.GetClass().AssemblyQualifiedName;
                        data.SceneName = GameMetadataStorage.GetScene(folder)?.name;
                        data.GameName = GameMetadataStorage.GetGameName(folder);
                        data.SceneBundleName = $"{data.GameName.Replace(' ', '_').ToLowerInvariant()}.scene.bundle";
                        data.AssetsBundleName = $"{data.GameName.Replace(' ', '_').ToLowerInvariant()}.assets.bundle";
                    }

                    yield return data;
                }
            }
        }
    }
}