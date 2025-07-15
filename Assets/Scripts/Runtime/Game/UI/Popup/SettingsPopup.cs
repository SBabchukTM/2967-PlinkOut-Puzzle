using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.UI.Data;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using Runtime.Game.UI;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Core.UI.Popup
{
    public class SettingsPopup : BasePopup
    {
        private UserDataService _userDataService;
        private IAudioService _audioService;
        private IUiService _uiService;

        [SerializeField] private SimpleButton _closeButton;
        [SerializeField] private SimpleButton _privacyPolicyButton;
        [SerializeField] private SimpleButton _termsButton;
        [SerializeField] private Slider _soundVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;

        [Inject]
        public void Construct(UserDataService userDataService, IAudioService audioService, IUiService uiService)
        {
            _userDataService = userDataService;
            _audioService = audioService;
            _uiService = uiService;
        }

        public override void DestroyPopup()
        {
            _closeButton.Button.onClick.RemoveAllListeners();
            _privacyPolicyButton.Button.onClick.RemoveAllListeners();
            _termsButton.Button.onClick.RemoveAllListeners();
            _soundVolumeSlider.onValueChanged.RemoveAllListeners();
            _musicVolumeSlider.onValueChanged.RemoveAllListeners();
            base.DestroyPopup();
        }

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            Initialize();
            return base.Show(data, cancellationToken);
        }

        public void Initialize()
        {
            var musicValue = _userDataService.GetUserData().SettingsData.MusicVolume;
            var soundValue = _userDataService.GetUserData().SettingsData.SoundVolume;
            _musicVolumeSlider.value = musicValue;
            _soundVolumeSlider.value = soundValue;
            _closeButton.Button.onClick.AddListener(DestroyPopup);
            _privacyPolicyButton.Button.onClick.AddListener(ShowPrivacyPolicyPopup);
            _termsButton.Button.onClick.AddListener(ShowTermsPopup);
            _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChange);
            _soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChange);
        }

        public void OnMusicVolumeChange(float value)
        {
            _userDataService.GetUserData().SettingsData.MusicVolume = value;
            _audioService.SetVolume(Audio.AudioType.Music, value);
        }

        public void OnSoundVolumeChange(float value)
        {
            _userDataService.GetUserData().SettingsData.SoundVolume = value;
            _audioService.SetVolume(Audio.AudioType.Sound, value);
        }

        private void ShowPrivacyPolicyPopup()
        {
            _uiService.ShowPopup(ConstPopups.PrivacyPolicyPopup);
        }

        private void ShowTermsPopup()
        {
            _uiService.ShowPopup(ConstPopups.TermsPopup);
        }
    }
}