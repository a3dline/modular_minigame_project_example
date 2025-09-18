using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace GameManager.Public
{
    public class GameManagerScope : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            
        }
    }
    
    public class GameManagerController : ScopedControllerWithResultBase<GameManagerScope>
    {
        public GameManagerController(IObjectResolver resolver) : base(resolver) { }

        protected override async UniTask OnFlowAsync(CancellationToken cancellationToken)
        {
            await UniTask.Delay(5000, cancellationToken: cancellationToken);
            throw new System.Exception("GameManagerController Exception");
        }
    }
}