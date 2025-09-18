using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.GameManager.Views;
using Playtika.Controllers;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.GameManager
{
    public struct GameSelectionViewData
    {
        public string Name;
    }
    
    internal class SelectGamePopupViewController : ControllerWithResultBase<IEnumerable<GameSelectionViewData>, GameSelectionViewData>
    {
        public SelectGamePopupViewController(IControllerFactory controllerFactory) : base(controllerFactory) { }
        
        private Dictionary<string, GameSelectionViewData> _gameMap; 

        protected override async UniTask OnFlowAsync(CancellationToken cancellationToken)
        {
            using var _ = DictionaryPool<string, GameSelectionViewData>.Get(out _gameMap);
            var viewPrefab = (SelectedGameView)await Resources.LoadAsync<SelectedGameView>("SelectedGamePopup");
            var view = Object.Instantiate(viewPrefab);
            view.OnGameSelected += OnGameSelected;
            
            foreach (var gameData in Args)
            {
                _gameMap[gameData.Name] = gameData;
                view.AddButton(gameData.Name);
            }
            
            await UniTask.WaitUntilCanceled(cancellationToken);
            Object.Destroy(view.gameObject);
        }

        private void OnGameSelected(string selectedGame)
        {
            var data = _gameMap[selectedGame];
            Complete(data);
        }
    }
}