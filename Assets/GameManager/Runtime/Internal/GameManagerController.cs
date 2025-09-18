using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Game.Common;
using Playtika.Controllers;
using VContainer.Unity;

namespace Game.GameManager
{
    internal class GameManagerController : ControllerWithResultBase, IGameManagerController
    {
        private readonly IGameManifestsProvider _gameManifestsProvider;
        private readonly ILoggerService _logger;

        public GameManagerController(IControllerFactory controllerFactory,
                                     IGameManifestsProvider gameManifestsProvider,
                                     ILoggerService logger) : base(controllerFactory)
        {
            _gameManifestsProvider = gameManifestsProvider;
            _logger = logger;
        }

        protected override async UniTask OnFlowAsync(CancellationToken cancellationToken)
        {
            var gameDataList = new List<GameSelectionViewData>();
            var manifests = _gameManifestsProvider.GetGameManifests().ToArray();
            var manifestMap = new Dictionary<string, IGameManifest>();
            foreach (var gameManifest in manifests)
            {
                var gameData = new GameSelectionViewData 
                {
                    Name = gameManifest.Name,
                };
                manifestMap[gameData.Name] = gameManifest;
                gameDataList.Add(gameData);
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
                throw new NullReferenceException($"Game manifest {manifest.Name} has null launcher type");
            }
            
            if (!typeof(IGameLauncher).IsAssignableFrom(manifest.LauncherType.Type))
            {
                throw new InvalidCastException($"Game manifest {manifest.Name} has invalid launcher type {manifest.LauncherType}");
            }

            if (manifest.ScopeType == null)
            {
                throw new NullReferenceException($"Game manifest {manifest.Name} has null scope type");
            }

            if (!typeof(IInstaller).IsAssignableFrom(manifest.ScopeType.Type))
            {
                throw new InvalidCastException($"Game manifest {manifest.Name} has invalid scope type {manifest.ScopeType}");
            }
        }
    }
}