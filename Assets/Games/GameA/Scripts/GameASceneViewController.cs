using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Playtika.Controllers;
using UnityEngine.SceneManagement;

namespace Games.GameA
{
    public class GameASceneViewController : ControllerWithResultBase<Scene, EmptyControllerResult>
    {
        public GameASceneViewController(IControllerFactory controllerFactory) : base(controllerFactory) { }

        protected override UniTask OnFlowAsync(CancellationToken cancellationToken)
        {
            var view = Args.GetRootGameObjects().Select(x => x.GetComponent<SceneAView>()).FirstOrDefault(x => x != null);
            if (view == null)
            {
                throw new Exception("SceneAView not found in scene");
            }

            view.OnCloseButtonClicked += () => Complete(default);
            return UniTask.CompletedTask;
        }
    }
}