using System;
using UnityEngine;

namespace Game.Common
{
    public class DisposableInstance : IDisposable
    {
        private readonly GameObject _gameObject;
        public DisposableInstance(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public void Dispose()
        {
            if (_gameObject != null)
            {
                UnityEngine.Object.Destroy(_gameObject);
            }
        }
    }
}