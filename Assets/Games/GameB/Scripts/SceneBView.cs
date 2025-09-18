using System;
using UnityEngine;
using UnityEngine.UI;

namespace Games.GameB
{
    public class SceneBView : MonoBehaviour
    {
        [SerializeField]
        private Button _closeButton;
        
        [SerializeField]
        private Button _throwExceptionButton;
        
        public event Action OnCloseRequested; 
        public event Action OnExceptionRequested;
        
        private void Awake()
        {
            _closeButton.onClick.AddListener(() => OnCloseRequested?.Invoke());
            _throwExceptionButton.onClick.AddListener(() => OnExceptionRequested?.Invoke());
        }
    }
}
