using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameManager.Views
{
    public class SelectedGameView : MonoBehaviour
    {
        [SerializeField]
        private Button _buttonTemplate;

        private void Awake()
        {
            _buttonTemplate.gameObject.SetActive(false);
        }

        public event Action<string> OnGameSelected;

        public void AddButton(string gameName)
        {
            var button = Instantiate(_buttonTemplate, _buttonTemplate.transform.parent);
            button.gameObject.SetActive(true);
            button.GetComponentInChildren<TMP_Text>().text = $"Start \"{gameName}\"";
            button.onClick.AddListener(() => OnGameSelected?.Invoke(gameName));
        }
    }
}