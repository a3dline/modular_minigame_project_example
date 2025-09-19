using UnityEngine.SceneManagement;

namespace Game.GameManager
{
    internal class GameSceneProviderFactory
    {
        public IGameSceneProvider Create(Scene scene)
        {
            return new GameSceneProvider(scene);
        }
    }
}