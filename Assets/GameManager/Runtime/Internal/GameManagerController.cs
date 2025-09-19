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
            var index = 0;
            foreach (var gameData in gameDatas)
            {
                var gameName = gameData.GameName ?? "Game " + (++index);
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

        
    }
}