using System;
using TMPro;
using UnityEngine;

namespace Runtime.Game.UI.Screen
{
    public class GameplayScreen : UiScreen
    {
        private const float FadeAnimTime = 0.5f;

        [SerializeField] private SimpleButton _backButton;
        [SerializeField] private SimpleButton _shootButton;
        [SerializeField] private SimpleButton _upgradeBuildingButton;
        [SerializeField] private TextMeshProUGUI _balanceText;
        [SerializeField] private UIHoldButton _leftButton;
        [SerializeField] private UIHoldButton _rightButton;
        [SerializeField] private BuildingUpgradeMenu _upgradeMenu;
        [SerializeField] private BallRecoverySystem _ballRecoverySystem;

        [SerializeField] private CameraAdjuster _cameraAdjuster;

        public event Action OnBackPressed;
        public event Action OnLeftPressed;
        public event Action OnRightPressed;
        public event Action OnShootPressed;
        public event Action OnUpgradeBuildingPressed;

        private void OnDisable()
        {
            UnsubscribeToEvents();
            _upgradeMenu.Close();
        }

        public void Initialize()
        {
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _backButton.Button.onClick.AddListener(() => OnBackPressed?.Invoke());
            _leftButton.OnHold += () => OnLeftPressed?.Invoke();
            _rightButton.OnHold += () => OnRightPressed?.Invoke();
            _shootButton.Button.onClick.AddListener(() => { OnShootPressed?.Invoke(); StartGame(); });
            _upgradeBuildingButton.Button.onClick.AddListener(() => _upgradeMenu.Open());
        }

        private void UnsubscribeToEvents()
        {
        }

        public void StartGame()
        {
            SetActiveShootButton(false);
            SetActiveUpgradeBuildingButton(false);
        }

        public void EndGame()
        {
            SetActiveShootButton(true);
            SetActiveUpgradeBuildingButton(true);
        }

        public void SetActiveShootButton(bool active)
        {
            _shootButton.Button.interactable = active;
        }

        public void SetActiveUpgradeBuildingButton(bool active)
        {
            _upgradeBuildingButton.Button.interactable = active;
        }

        public void DisableBackButton() => _backButton.Button.interactable = false;

        public void SetBalance(int balance) => _balanceText.text = balance.ToString();

        public CameraAdjuster GetCameraAdjuster() => _cameraAdjuster;

        public BuildingUpgradeMenu GetBuildingUpgradeMenu() => _upgradeMenu;

        public BallRecoverySystem GetBallRecoverySystem() => _ballRecoverySystem;
    }
}