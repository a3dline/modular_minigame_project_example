using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameManager.Editor;
using UnityEditor;

namespace Game.GameManager
{
    internal class EditorGameManifestProvider : IGameManifestsProvider
    {
        public IEnumerable<IGameManifest> GetGameManifests()
        {
            var foldersEnumerator = AssetDatabase.FindAssets("t:DefaultAsset").Select(AssetDatabase.GUIDToAssetPath);
            foreach (var folder in foldersEnumerator)
            {
                if (GameMetadataStorage.IsGameEnabled(folder))
                {
                    var manifest = GameMetadataStorage.GetManifest(folder);
                    if (manifest != null)
                    {
                        yield return Activator.CreateInstance(manifest.GetClass()) as IGameManifest;
                    }
                }
            }
        }
    }
}