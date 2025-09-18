using UnityEditor;
using UnityEngine.UIElements;
using VContainer;

namespace Game.GameManager.Editor
{
    [CustomEditor(typeof(DefaultAsset), true)]
    internal class FolderInspectorOverride : UnityEditor.Editor
    {
        private IObjectResolver _scope;

        private void OnDestroy()
        {
            _scope.Dispose();
        }

        public override VisualElement CreateInspectorGUI()
        {
            var container = new ContainerBuilder();
            container.Register<DirectoryPathProvider>(Lifetime.Singleton)
                     .WithParameter("targetObject", target);
            container.Register<GameManagerEditor>(Lifetime.Transient);

            _scope = container.Build();

            var editor = _scope.Resolve<GameManagerEditor>();
            editor.Repaint();
            return editor;
        }
    }
}