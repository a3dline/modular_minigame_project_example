using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.GameManager.Editor
{
    internal class GameManagerEditor : VisualElement
    {
        private readonly string _directoryPath;

        public GameManagerEditor(DirectoryPathProvider pathProvider)
        {
            _directoryPath = pathProvider.GetDirectoryPath();
        }

        public void Repaint()
        {
            Clear();
            var isEnabled = GameMetadataStorage.IsGameEnabled(_directoryPath);

            if (!isEnabled)
            {
                if (IsDirectoryHierarchyHasGame() || IsChildrenHasGame(_directoryPath))
                {
                    return;
                }
            }
            
            var enableButton = new Button(() =>
            {
                if (isEnabled)
                {
                    GameMetadataStorage.DisableGame(_directoryPath);
                }
                else
                {
                    GameMetadataStorage.EnableGame(_directoryPath);
                }

                Repaint();
            }) { text = isEnabled ? "Disable Game" : "Enable Game" };

            Add(enableButton);
            
            if (!isEnabled)
            {
                return;
            }

            var manifestField = new ObjectField("Manifest");
            manifestField.objectType = typeof(MonoScript);
            var manifest = GameMetadataStorage.GetManifest(_directoryPath);
            if (manifest is not null)
            {
                manifestField.SetValueWithoutNotify(manifest);
            }

            manifestField.RegisterValueChangedCallback(evt =>
            {
                var newValue = evt.newValue as MonoScript;
                if (Validate(newValue))
                {
                    GameMetadataStorage.SetManifest(_directoryPath, newValue);
                }
                else
                {
                    Debug.LogError("Invalid manifest. It must be a MonoScript implementing IGameManifest interface.");
                    manifestField.SetValueWithoutNotify(manifest);
                }
            });

            var sceneField = new ObjectField("Initial Scene");
            sceneField.objectType = typeof(SceneAsset);
            sceneField.SetValueWithoutNotify(GameMetadataStorage.GetScene(_directoryPath));
            sceneField.RegisterValueChangedCallback(evt =>
            {
                var newValue = evt.newValue as SceneAsset;
                GameMetadataStorage.SetScene(_directoryPath, newValue);
            });
            
            var gameName = new TextField("Game Name");
            gameName.SetValueWithoutNotify(GameMetadataStorage.GetGameName(_directoryPath));
            gameName.RegisterValueChangedCallback(evt =>
            {
                var newValue = evt.newValue;
                GameMetadataStorage.SetGameName(_directoryPath, newValue);
            });

            Add(gameName);
            Add(manifestField);
            Add(sceneField);
        }

        private bool IsDirectoryHierarchyHasGame()
        {
            var parent = Directory.GetParent(_directoryPath);
            var stopPath = Path.GetFullPath(Application.dataPath);

            while (parent != null)
            {
                var current = Path.GetFullPath(parent.FullName);
                if (string.Equals(current, stopPath, System.StringComparison.OrdinalIgnoreCase))
                    break;

                if (GameMetadataStorage.IsGameEnabled(current))
                    return true;

                parent = parent.Parent;
            }

            return false;
        }
        
        private bool IsChildrenHasGame(string directoryPath)
        {
            var directories = Directory.GetDirectories(directoryPath);
            foreach (var directory in directories)
            {
                if (GameMetadataStorage.IsGameEnabled(directory) || IsChildrenHasGame(directory))
                {
                    return true;
                }
            }

            return false;
        }

        private bool Validate(MonoScript manifest)
        {
            var type = manifest.GetClass();
            return manifest != null && type != null && typeof(IGameManifest).IsAssignableFrom(type);
        }
    }
}