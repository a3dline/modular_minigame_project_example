using System;
using UnityEngine;
using UnityEngine.UI;

namespace Games.GameB
{
    public class GameBPopupView : MonoBehaviour
    {
        [SerializeField]
        private Button _closeButton;
        
        [SerializeField]
        private Button _throwExceptionButton;
        
        public event Action OnCloseButtonClicked;
        public event Action OnThrowExceptionButtonClicked;
        
        private void Awake()
        {
            _closeButton.onClick.AddListener(() => OnCloseButtonClicked?.Invoke());
            _throwExceptionButton.onClick.AddListener(() => OnThrowExceptionButtonClicked?.Invoke());
        }
    }
}