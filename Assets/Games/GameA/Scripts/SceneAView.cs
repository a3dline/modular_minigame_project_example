using System;
using UnityEngine;
using UnityEngine.UI;

namespace Games.GameA
{
    public class SceneAView : MonoBehaviour
    {
        [SerializeField]
        private Button _closeButton;

        private void Awake()
        {
            _closeButton.onClick.AddListener(() => OnCloseButtonClicked?.Invoke());
        }

        public event Action OnCloseButtonClicked;
    }
}