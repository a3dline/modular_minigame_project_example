using UnityEngine.SceneManagement;

namespace Game.GameManager
{
    public interface IGameSceneProvider
    {
        Scene Scene { get; }
    }
}