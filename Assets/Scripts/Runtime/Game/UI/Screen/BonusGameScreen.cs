using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class BonusGameScreen : UiScreen
    {

        private const float FadeAnimTime = 0.5f;

        [SerializeField] private SimpleButton _backButton;
        [SerializeField] private SimpleButton _shootButton;

        [SerializeField] private TextMeshProUGUI _balanceText;
        [SerializeField] private UIHoldButton _leftButton;
        [SerializeField] private UIHoldButton _rightButton;
        [SerializeField] private BallRecoverySystem _ballRecoverySystem;

        [SerializeField] private CameraAdjuster _cameraAdjuster;

        public event Action OnBackPressed;
        public event Action OnLeftPressed;
        public event Action OnRightPressed;
        public event Action OnShootPressed;

        private void OnDisable()
        {
            UnsubscribeToEvents();
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
            _shootButton.Button.onClick.AddListener(() => { OnShootPressed?.Invoke(); StartBonusGame(); });
        }

        private void UnsubscribeToEvents()
        {
        }

        public void StartBonusGame()
        {
            _shootButton.Button.interactable = false;
        }

        public void EndBonusGame()
        {
            _shootButton.Button.interactable = true;
        }

        public void DisableBackButton() => _backButton.Button.interactable = false;

        public void SetBalance(int balance) => _balanceText.text = balance.ToString();

        public CameraAdjuster GetCameraAdjuster() => _cameraAdjuster;

        public BallRecoverySystem GetBallRecoverySystem() => _ballRecoverySystem;
    }

}