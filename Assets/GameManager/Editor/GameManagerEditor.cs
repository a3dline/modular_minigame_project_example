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
            
            Add(enableButton);
            Add(manifestField);
        }

        private bool Validate(MonoScript manifest)
        {
            var type = manifest.GetClass();
            return manifest != null && type != null && typeof(IGameManifest).IsAssignableFrom(type);
        }
    }
}