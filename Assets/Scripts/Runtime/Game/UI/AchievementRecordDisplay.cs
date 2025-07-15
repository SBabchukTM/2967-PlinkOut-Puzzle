using Runtime.Core.Data;
using Runtime.Core.UI.Popup;
using Runtime.Game.UI;
using System;
using TMPro;
using UnityEngine;

namespace Runtime.Game
{
    public class AchievementRecordDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private int _reward;
        [SerializeField] private SimpleButton _button;
        [SerializeField] private TextMeshProUGUI _buttonText;

        private AchievementRecord _achievementRecord;

        public event Action<AchievementType, int> OnClaimButtonPressed;

        public void Initialize(AchievementRecord achievementRecord)
        {
            _achievementRecord = achievementRecord;
            AchievementType achievementType = achievementRecord.Config.AchievementType;
            string title = achievementRecord.Config.Name;
            string description = achievementRecord.Config.Description;
            int reward = achievementRecord.Config.Reward;

            _name.text = title;
            _description.text = description;
            _reward = reward;
            _rewardText.text = _reward.ToString();
            _button.Button.onClick.RemoveAllListeners();
            _button.Button.onClick.AddListener(() => OnClaimButtonPressed?.Invoke(achievementType, reward));
            UpdateButtonStatus();
        }

        public void RefreshDisplay()
        {
            UpdateButtonStatus();
        }

        private void UpdateButtonStatus()
        {
            AchievementStatus status = _achievementRecord.Progress.achievementStatus;
            switch (status)
            {
                case AchievementStatus.InProgress:
                    _buttonText.text = "InProgress";
                    _button.Button.interactable = false;
                    break;

                case AchievementStatus.Completed:
                    _buttonText.text = $"Claim";
                    _button.Button.interactable = true;
                    break;

                case AchievementStatus.Claimed:
                    _buttonText.text = "Claimed";
                    _button.Button.interactable = false;
                    break;
            }
        }
    }
}