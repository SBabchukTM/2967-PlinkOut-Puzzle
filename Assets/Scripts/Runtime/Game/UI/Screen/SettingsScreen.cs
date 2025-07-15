using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class SettingsScreen : UiScreen
    {
        [SerializeField] private SimpleButton _backButton;
        [SerializeField] private Toggle _soundVolumeToggle;
        [SerializeField] private Toggle _musicVolumeToggle;

        public event Action OnBackButtonClicked;
        public event Action<bool> OnSoundVolumeChangeEvent;
        public event Action<bool> OnMusicVolumeChangeEvent;

        private void OnDestroy()
        {
            _backButton.Button.onClick.RemoveAllListeners();
            _soundVolumeToggle.onValueChanged.RemoveAllListeners();
            _musicVolumeToggle.onValueChanged.RemoveAllListeners();
        }

        public void Initialize(bool musicValue, bool soundValue)
        {
            _musicVolumeToggle.isOn = musicValue;
            _soundVolumeToggle.isOn = soundValue;
            _backButton.Button.onClick.AddListener(() => OnBackButtonClicked?.Invoke());
            _soundVolumeToggle.onValueChanged.AddListener((value) => OnSoundVolumeChangeEvent?.Invoke(value));
            _musicVolumeToggle.onValueChanged.AddListener((value) => OnMusicVolumeChangeEvent?.Invoke(value));
        }
    }
}