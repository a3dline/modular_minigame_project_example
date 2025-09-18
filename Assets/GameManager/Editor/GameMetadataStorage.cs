using AMetadataStorage;
using UnityEditor;
using UnityEngine;

namespace Game.GameManager.Editor
{
    internal static class GameMetadataStorage
    {
        private const string EnabledKey = "gamemanager_enabled";
        private const string ManifestKey = "gamemanager_manifest";
        
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
            if (UserMetadataStorage.Load(directoryPath, ManifestKey, out var value))
            {
                if(GlobalObjectId.TryParse(value, out var id))
                {
                    var objectId = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(id);
                    return objectId as MonoScript;
                }
            }

            return null;
        }
        
        public static void SetManifest(string directoryPath, MonoScript manifest)
        {
            if (manifest == null)
            {
                UserMetadataStorage.Remove(directoryPath, ManifestKey);
            }
            else
            {
                var id = GlobalObjectId.GetGlobalObjectIdSlow(manifest);
                UserMetadataStorage.Upsert(directoryPath, ManifestKey, id.ToString());
            }
        }
    }
}