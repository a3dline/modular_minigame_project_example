using System.Threading;
using Cysharp.Threading.Tasks;
using Playtika.Controllers;

namespace Game.GameManager
{
    public interface IGameControllerRunner
    {
        public UniTask<TResult> ExecuteAndWaitResultAsync<T, TArg, TResult>(TArg arg, CancellationToken token)
            where T : class, IControllerWithResult<TResult>, IController<TArg>;

        public UniTask ExecuteAndWaitResultAsync<T, TArg>(TArg arg, CancellationToken token)
            where T : class, IControllerWithResult<EmptyControllerResult>, IController<TArg>;

        public UniTask<TResult> ExecuteAndWaitResultAsync<T, TResult>(CancellationToken token)
            where T : class, IControllerWithResult<TResult>, IController<EmptyControllerArg>;

        public UniTask ExecuteAndWaitResultAsync<T>(CancellationToken token) 
            where T : class, IControllerWithResult<EmptyControllerResult>, IController<EmptyControllerArg>;
    }
}