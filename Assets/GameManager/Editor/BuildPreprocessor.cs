using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Game.GameManager.Editor
{
    public class BuildPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }

        public void OnPreprocessBuild(BuildReport report)
        {
            var allGameData = new List<GameData>();
            var foldersEnumerator = AssetDatabase.FindAssets("t:DefaultAsset").Select(AssetDatabase.GUIDToAssetPath);

            foreach (var folder in foldersEnumerator)
            {
                if (GameMetadataStorage.IsGameEnabled(folder))
                {
                    var assets = GameMetadataStorage.GetAllAssets(folder).Where(x => x != null).Distinct().ToArray();
                    var data = new GameData();

                    var manifest = GameMetadataStorage.GetManifest(folder);
                    if (manifest != null)
                    {
                        data.ManifestType = manifest.GetClass().AssemblyQualifiedName;
                        data.SceneName = GameMetadataStorage.GetScene(folder)?.name;
                        data.GameName = GameMetadataStorage.GetGameName(folder);
                        data.SceneBundleName = $"{data.GameName.Replace(' ', '_').ToLowerInvariant()}.scene.bundle";
                        if (assets.Length > 0)
                        {
                            data.AssetsBundleName = $"{data.GameName.Replace(' ', '_').ToLowerInvariant()}.assets.bundle";
                        }
                    }

                    allGameData.Add(data);
                }
            }

            var gameDataStorage = ScriptableObject.CreateInstance<GameDataStorage>();
            gameDataStorage.Data = allGameData.ToArray();
            var gameDataPath = $"Assets/Resources/{GameDataStorage.FileName}.asset";
            if (!Directory.Exists(Path.GetDirectoryName(gameDataPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(gameDataPath)!);
            }

            AssetDatabase.CreateAsset(gameDataStorage, gameDataPath);
        }
    }
}