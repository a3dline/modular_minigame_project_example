using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.GameManager;
using Playtika.Controllers;
using UnityEngine.SceneManagement;

namespace Games.GameA
{
    public class GameASceneViewController : ControllerWithResultBase
    {
        private readonly IGameSceneProvider _sceneProvider;

        public GameASceneViewController(IControllerFactory controllerFactory,
                                        IGameSceneProvider sceneProvider) 
            : base(controllerFactory)
        {
            _sceneProvider = sceneProvider;
        }

        protected override UniTask OnFlowAsync(CancellationToken cancellationToken)
        {
            var view = _sceneProvider.Scene
                                     .GetRootGameObjects()
                                     .Select(x => x.GetComponent<SceneAView>())
                                     .FirstOrDefault(x => x != null);
            if (view == null)
            {
                throw new Exception("SceneAView not found in scene");
            }

            view.OnCloseButtonClicked += () => Complete(default);
            return UniTask.CompletedTask;
        }
    }
}