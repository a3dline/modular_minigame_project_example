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
        
        private static T GetObject<T>(string directoryPath, string key) where T : Object
        {
            if (UserMetadataStorage.Load(directoryPath, key, out var value))
            {
                if(GlobalObjectId.TryParse(value, out var id))
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