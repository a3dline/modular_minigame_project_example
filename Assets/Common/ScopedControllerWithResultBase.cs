using Playtika.Controllers;
using VContainer;
using VContainer.Unity;

namespace Game.Common
{
    public abstract class ScopedControllerWithResultBase<TScope> : ControllerWithResultBase
        where TScope : IInstaller, new()
    {
        public ScopedControllerWithResultBase(IObjectResolver resolver)
            : base(ScopeBuilder.Create(resolver, out var scope))
        {
            AddDisposable(scope);
        }

        private static class ScopeBuilder
        {
            public static IControllerFactory Create(IObjectResolver resolver, out IObjectResolver scope)
            {
                var installer = new TScope();
                scope = resolver.CreateScope(builder => { installer.Install(builder); });

                return scope.Resolve<IControllerFactory>();
            }
        }
    }
}