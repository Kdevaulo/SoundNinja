using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Kdevaulo.SoundNinja.SoundMenuBehaviour.SoundsContainerSystem
{
    public sealed class SoundView : MonoBehaviour
    {
        public event Action SoundPlayClicked = delegate { };
        public event Action RemoveSoundClicked = delegate { };
        public event Action StartGameClicked = delegate { };

        [SerializeField] private TextMeshProUGUI _textMeshPro;

        [SerializeField] private Button _playSoundButton;

        [SerializeField] private Button _removeSoundButton;

        [SerializeField] private Button _startGameButton;

        private void OnDestroy()
        {
            _playSoundButton.onClick.RemoveListener(HandlePlayButtonClick);
            _removeSoundButton.onClick.RemoveListener(HandleRemoveButtonClick);
            _startGameButton.onClick.RemoveListener(HandleStartGameButtonClick);
        }

        public void Initialize()
        {
            _playSoundButton.onClick.AddListener(HandlePlayButtonClick);
            _removeSoundButton.onClick.AddListener(HandleRemoveButtonClick);
            _startGameButton.onClick.AddListener(HandleStartGameButtonClick);
        }

        public void SetText(string text)
        {
            _textMeshPro.text = text;
        }

        private void HandlePlayButtonClick()
        {
            SoundPlayClicked.Invoke();
        }

        private void HandleRemoveButtonClick()
        {
            RemoveSoundClicked.Invoke();
        }

        private void HandleStartGameButtonClick()
        {
            StartGameClicked.Invoke();
        }
    }
}