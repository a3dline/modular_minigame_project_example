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
            var gameDatas = _gameDataProvider.GetAllGamesData().ToArray();
            var gameDataMap = new Dictionary<string, GameData>();
            foreach (var gameData in gameDatas)
            {
                ValidateGameData(gameData);
                var gameName = gameData.GameName ?? gameData.Manifest.GetType().Name;
                var viewData = new GameSelectionViewData { Name = gameName };
                gameDataMap[gameName] = gameData;
                gameDataList.Add(viewData);
            }

            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate().WithCancellation(cancellationToken))
            {
                var selectedGame = await ExecuteAndWaitResultAsync<SelectGamePopupViewController, IEnumerable<GameSelectionViewData>, GameSelectionViewData>(gameDataList, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                var gameData = gameDataMap[selectedGame.Name];

                await ExecuteAndWaitResultAsync<GameControllerRunner, GameData, EmptyControllerResult>(gameData, cancellationToken);
                await UniTask.Delay(100, cancellationToken: cancellationToken);
            }
        }

        private void ValidateGameData(GameData gameData)
        {
            var manifest = gameData.Manifest;
            if (manifest == null)
            {
                throw new NullReferenceException("Game data has null manifest");
            }

            if (manifest.LauncherType == null)
            {
                throw new NullReferenceException($"Game manifest {manifest.GetType().Name} has null launcher type");
            }

            if (!typeof(IGameLauncher).IsAssignableFrom(manifest.LauncherType.Type))
            {
                throw new InvalidCastException($"Game manifest {manifest.GetType().Name} has invalid launcher type {manifest.LauncherType}");
            }

            if (manifest.ScopeType != null && !typeof(IInstaller).IsAssignableFrom(manifest.ScopeType.Type))
            {
                throw new InvalidCastException($"Game manifest {manifest.GetType().Name} has invalid scope type {manifest.ScopeType}");
            }

            if (string.IsNullOrEmpty(gameData.SceneName))
            {
                throw new ArgumentException($"Game manifest {manifest.GetType().Name} has empty scene name");
            }
        }
    }
}