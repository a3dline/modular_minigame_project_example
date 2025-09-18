using UnityEditor;
using UnityEngine;

namespace Game.GameManager.Editor
{
    
    
    internal class DirectoryPathProvider
    {
        private readonly Object _targetObject;
        public DirectoryPathProvider(Object targetObject)
        {
            _targetObject = targetObject;
        }

        public string GetDirectoryPath()
        {
            return AssetDatabase.GetAssetPath(_targetObject);
        }
    }
}