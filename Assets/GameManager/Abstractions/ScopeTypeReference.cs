using System;
using VContainer.Unity;

namespace Game.GameManager
{
    public class ScopeTypeReference<TScope> : ITypeReference
        where TScope : IInstaller, new()
    {
        public Type Type => typeof(TScope);
    }
}