using UnityEngine.SceneManagement;

namespace Game.GameManager
{
    internal class GameSceneProvider : IGameSceneProvider
    {
        public GameSceneProvider(Scene scene)
        {
            Scene = scene;
        }

        public Scene Scene { get; }
    }
}