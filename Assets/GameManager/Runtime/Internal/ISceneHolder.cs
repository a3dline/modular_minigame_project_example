using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Game.GameManager
{
    internal interface ISceneHolder : IUniTaskAsyncDisposable
    {
        Scene Scene { get; }
    }
}