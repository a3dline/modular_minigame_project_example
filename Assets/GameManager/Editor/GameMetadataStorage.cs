using System.Collections.Generic;
using AMetadataStorage;
using UnityEditor;
using UnityEngine;

namespace Game.GameManager.Editor
{
    internal static class GameMetadataStorage
    {
        private const string EnabledKey = "gamemanager_enabled";
        private const string ManifestKey = "gamemanager_manifest";
        private const string SceneKey = "gamemanager_scene";
        private const string GameNameKey = "gamemanager_name";
        private const string AssetsKey = "gamemanager_assets";
        private const string AssetsCountKey = "gamemanager_assets_count";

        public static void EnableGame(string directoryPath)
        {
            UserMetadataStorage.Upsert(directoryPath, EnabledKey, "true");
        }

        public static void DisableGame(string directoryPath)
        {
            UserMetadataStorage.Remove(directoryPath, EnabledKey);
        }

        public static bool IsGameEnabled(string directoryPath)
        {
            return UserMetadataStorage.Load(directoryPath, EnabledKey, out var value) && value == "true";
        }

        public static MonoScript GetManifest(string directoryPath)
        {
            return GetObject<MonoScript>(directoryPath, ManifestKey);
        }

        public static void SetManifest(string directoryPath, MonoScript manifest)
        {
            SetObject(directoryPath, ManifestKey, manifest);
        }

        public static void SetScene(string directoryPath, SceneAsset scene)
        {
            SetObject(directoryPath, SceneKey, scene);
        }

        public static SceneAsset GetScene(string directoryPath)
        {
            return GetObject<SceneAsset>(directoryPath, SceneKey);
        }

        public static void SetGameName(string directoryPath, string gameName)
        {
            if (string.IsNullOrEmpty(gameName))
            {
                UserMetadataStorage.Remove(directoryPath, GameNameKey);
            }
            else
            {
                UserMetadataStorage.Upsert(directoryPath, GameNameKey, gameName);
            }
        }

        public static string GetGameName(string directoryPath)
        {
            if (UserMetadataStorage.Load(directoryPath, GameNameKey, out var value))
            {
                return value;
            }

            return null;
        }

        public static List<Object> GetAllAssets(string directoryPath)
        {
            var sourceCount = GetAssetsCount(directoryPath);

            var result = new List<Object>();
            for (var i = 0; i < sourceCount; i++)
            {
                var exist = UserMetadataStorage.Load(directoryPath, $"{AssetsKey}_{i}", out var value);
                if (!exist)
                {
                    result.Add(null);
                    continue;
                }

                if (GlobalObjectId.TryParse(value, out var id))
                {
                    var objectId = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id);
                    if (objectId != null)
                    {
                        result.Add(objectId);
                    }
                }
                else
                {
                    result.Add(null);   
                }
            }

            return result;
        }

        public static void SetAllAssets(string directoryPath, List<Object> assets)
        {
            var currentAssetCount = GetAssetsCount(directoryPath);
            var sizeDelta = currentAssetCount - assets.Count;

            if (sizeDelta > 0)
            {
                for (var i = assets.Count; i < currentAssetCount; i++)
                {
                    UserMetadataStorage.Remove(directoryPath, $"{AssetsKey}_{i}");
                }    
            }

            for (var i = 0; i < assets.Count; i++)
            {
                var asset = assets[i];

                var id = GlobalObjectId.GetGlobalObjectIdSlow(asset);
                UserMetadataStorage.Upsert(directoryPath, $"{AssetsKey}_{i}", id.ToString());
            }
            
            SetAssetsCount(directoryPath, assets.Count);;
        }

        private static int GetAssetsCount(string directoryPath)
        {
            if (UserMetadataStorage.Load(directoryPath, AssetsCountKey, out var value) && int.TryParse(value, out var count))
            {
                return count;
            }

            return 0;
        }

        private static void SetAssetsCount(string directoryPath, int count)
        {
            UserMetadataStorage.Upsert(directoryPath, AssetsCountKey, count.ToString());
        }

        private static T GetObject<T>(string directoryPath, string key) where T : Object
        {
            if (UserMetadataStorage.Load(directoryPath, key, out var value))
            {
                if (GlobalObjectId.TryParse(value, out var id))
                {
                    var objectId = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id);
                    return objectId as T;
                }
            }

            return null;
        }

        private static void SetObject<T>(string directoryPath, string key, T obj) where T : Object
        {
            if (obj == null)
            {
                UserMetadataStorage.Remove(directoryPath, key);
            }
            else
            {
                var id = GlobalObjectId.GetGlobalObjectIdSlow(obj);
                UserMetadataStorage.Upsert(directoryPath, key, id.ToString());
            }
        }
    }
}