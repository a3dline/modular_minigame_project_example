using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Common;
using Playtika.Controllers;
using UnityEngine;
using VContainer;

namespace Games.GameA
{
    public class GameAController : ControllerWithResultBase
    {
        public GameAController(IControllerFactory controllerFactory) : base(controllerFactory) { }

        protected override UniTask OnFlowAsync(CancellationToken cancellationToken)
        {
            Debug.Log("Game Stqarted");
            return UniTask.Delay(1000, cancellationToken: cancellationToken);
        }
    }
}