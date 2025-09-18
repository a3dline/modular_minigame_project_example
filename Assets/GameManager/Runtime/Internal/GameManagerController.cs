using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Playtika.Controllers;
using VContainer.Unity;

namespace Game.GameManager
{
    internal class GameManagerController : ControllerWithResultBase, IGameManagerController
    {
        private readonly IGameDataProvider _gameDataProvider;

        public GameManagerController(IControllerFactory controllerFactory,
                                     IGameDataProvider gameDataProvider)
            : base(controllerFactory)
        {
            _gameDataProvider = gameDataProvider;
        }

        protected override async UniTask OnFlowAsync(CancellationToken cancellationToken)
        {
            var gameDataList = new List<GameSelectionViewData>();
            var manifests = _gameDataProvider.GetAllGamesData().ToArray();
            var manifestMap = new Dictionary<string, IGameManifest>();
            foreach (var gameData in manifests)
            {
                var gameName = gameData.GameName?? gameData.Manifest.GetType().Name;
                var viewData = new GameSelectionViewData 
                {
                    Name = gameName
                };
                manifestMap[gameName] = gameData.Manifest;
                gameDataList.Add(viewData);
            }
            
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate().WithCancellation(cancellationToken))
            {
                var selectedGame = await ExecuteAndWaitResultAsync<SelectGamePopupViewController, IEnumerable<GameSelectionViewData>, GameSelectionViewData>(gameDataList, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                var manifest = manifestMap[selectedGame.Name];
                ValidateManifest(manifest);
                
                
                
                await ExecuteAndWaitResultAsync<GameControllerRunner, IGameManifest, EmptyControllerResult>(manifest, cancellationToken);
                await UniTask.Delay(100, cancellationToken: cancellationToken);
            }
        }

        private void ValidateManifest(IGameManifest manifest)
        {
            if (manifest.LauncherType == null)
            {
                throw new NullReferenceException($"Game manifest {manifest.GetType().Name} has null launcher type");
            }
            
            if (!typeof(IGameLauncher).IsAssignableFrom(manifest.LauncherType.Type))
            {
                throw new InvalidCastException($"Game manifest {manifest.GetType().Name} has invalid launcher type {manifest.LauncherType}");
            }

            if (manifest.ScopeType == null)
            {
                throw new NullReferenceException($"Game manifest {manifest.GetType().Name} has null scope type");
            }

            if (!typeof(IInstaller).IsAssignableFrom(manifest.ScopeType.Type))
            {
                throw new InvalidCastException($"Game manifest {manifest.GetType().Name} has invalid scope type {manifest.ScopeType}");
            }
        }
    }
}