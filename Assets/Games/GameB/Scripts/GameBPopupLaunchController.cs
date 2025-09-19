using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Common;
using Game.GameManager;
using Playtika.Controllers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Games.GameB
{
    public class GameBPopupLaunchController : ControllerWithResultBase<Transform, EmptyControllerResult>
    {
        private readonly IGameAssetsProvider _assetsProvider;
        private GameObject _instance;
        
        public GameBPopupLaunchController(IControllerFactory controllerFactory,
                                          IGameAssetsProvider assetsProvider)
            : base(controllerFactory)
        {
            _assetsProvider = assetsProvider;
        }

        protected override async UniTask OnFlowAsync(CancellationToken cancellationToken)
        {
            var popupPrefab = await _assetsProvider.LoadAsync<GameObject>("Popup", cancellationToken);
            _instance = Object.Instantiate(popupPrefab, Args);
            AddDisposable(new DisposableInstance(_instance));
            var view = _instance.GetComponent<GameBPopupView>();
            view.OnCloseButtonClicked += () => Complete(default);
            view.OnThrowExceptionButtonClicked += () => Fail(new Exception("GameB Popup throw exception"));
        }
    }
}