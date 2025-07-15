using System;
using UnityEngine;

namespace Runtime.Game.UI.Screen
{
    public class MenuScreen : UiScreen
    {
        [SerializeField] private SimpleButton _settingsButton;
        [SerializeField] private SimpleButton _infoButton;
        [SerializeField] private SimpleButton _achievementButton;
        [SerializeField] private SimpleButton _dailyRewardButton;
        //[SerializeField] private SimpleButton _privacyButton;
        //[SerializeField] private SimpleButton _termsButton;
        [SerializeField] private SimpleButton _playButton;
        [SerializeField] private SimpleButton _accountButton;
        [SerializeField] private SimpleButton _leaderboardButton;


        public event Action OnSettingsPressed;
        public event Action OnInfoPressed;
        public event Action OnAchievementPressed;
        public event Action OnDailyRewardPressed;
        //public event Action OnPrivacyPressed;
        //public event Action OnTermsPressed;
        public event Action OnPlayPressed;
        public event Action OnAccountPressed;
        public event Action OnLeaderboardPressed;

        private void OnDestroy()
        {
            _settingsButton.Button.onClick.RemoveAllListeners();
            _infoButton.Button.onClick.RemoveAllListeners();
            _achievementButton.Button.onClick.RemoveAllListeners();
            _dailyRewardButton.Button.onClick.RemoveAllListeners();
            //_privacyButton.Button.onClick.RemoveAllListeners();
            //_termsButton.Button.onClick.RemoveAllListeners();
            _playButton.Button.onClick.RemoveAllListeners();
            _accountButton.Button.onClick.RemoveAllListeners();
            _leaderboardButton.Button.onClick.RemoveAllListeners();
        }

        public void Initialize()
        {
            _settingsButton.Button.onClick.AddListener(() => OnSettingsPressed?.Invoke());
            _infoButton.Button.onClick.AddListener(() => OnInfoPressed?.Invoke());
            _achievementButton.Button.onClick.AddListener(() => OnAchievementPressed?.Invoke());
            _dailyRewardButton.Button.onClick.AddListener(() => OnDailyRewardPressed?.Invoke());
            //_privacyButton.Button.onClick.AddListener(() => OnPrivacyPressed?.Invoke());
            //_termsButton.Button.onClick.AddListener(() => OnTermsPressed?.Invoke());
            _playButton.Button.onClick.AddListener(() => OnPlayPressed?.Invoke());
            _accountButton.Button.onClick.AddListener(() => OnAccountPressed?.Invoke());
            _leaderboardButton.Button.onClick.AddListener(() => OnLeaderboardPressed?.Invoke());
        }
    }
}