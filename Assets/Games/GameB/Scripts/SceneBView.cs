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
        
        [SerializeField]
        private Button _displayPopupButton;
        
        public event Action OnCloseRequested; 
        public event Action OnExceptionRequested;
        public event Action OnDisplayPopupRequested; 
        
        private void Awake()
        {
            _closeButton.onClick.AddListener(() => OnCloseRequested?.Invoke());
            _throwExceptionButton.onClick.AddListener(() => OnExceptionRequested?.Invoke());
            _displayPopupButton.onClick.AddListener(() => OnDisplayPopupRequested?.Invoke());
        }
    }
}
