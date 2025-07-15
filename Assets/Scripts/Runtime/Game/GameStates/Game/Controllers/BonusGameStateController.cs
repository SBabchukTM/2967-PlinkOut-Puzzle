using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Game.Services.AchievementSystem;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData.Data;
using Runtime.Game.Services.UserData;
using Runtime.Game.UI.Screen;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;
using System.Threading.Tasks;
using Runtime.Game.GameStates.Game.Menu;
using Runtime.Core.UI.Popup;
using Runtime.Core.UI.Data;

namespace Runtime.Game.GameStates.Game.Controllers
{
    public class BonusGameStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly IAudioService _audioService;
        private readonly UserDataService _userDataService;
        private readonly IAssetProvider _assetProvider;

        private BonusGameScreen _screen;
        private UserInventory _userInventory;
        private PlayerStatsData _playerStatsData;

        private GameObject _gameFieldPrefab;
        private GameFieldController _gameFieldController;
        private BallRecoverySystem _ballRecoverySystem;

        private CancellationTokenSource _cts;

        private bool _gameEnded;
        private int _level;
        private int _balance;
        private bool _isBonusGame = false;
        private bool _isBallOnField = false;

        public BonusGameStateController(ILogger logger, IUiService uiService, IAudioService audioService, IAssetProvider assetProvider,
                UserDataService userDataService) : base(logger)
        {
            _uiService = uiService;
            _assetProvider = assetProvider;
            _audioService = audioService;
            _userDataService = userDataService;
        }

        public void Initialize()
        {
            _userInventory = _userDataService.GetUserData().UserInventory;
            _playerStatsData = _userDataService.GetUserData().PlayerStatsData;
        }

        public override async UniTask Enter(CancellationToken cancellationToken = default)
        {
            Initialize();
            _balance = 0;

            _gameEnded = false;

            CreateScreen();

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            _cts = new CancellationTokenSource();
            _ballRecoverySystem.StartBonusGame();

            await InitGameFieldAsync(_cts.Token);

            _gameFieldController.LuckyWheelController.gameObject.SetActive(false);

            SubscribeToEvents();
            await ShowBonusGamePopup();
            _isBallOnField = false;
            CheckScreen();
        }

        public override async UniTask Exit()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            UnsubscribeToEvents();

            _gameFieldController.Destroy();
            await _uiService.HideScreen(ConstScreens.BonusGameScreen);
        }

        public void SetLevel(int level) =>
                _level = level;

        private async Task InitGameFieldAsync(CancellationToken token)
        {
            _gameFieldPrefab = await _assetProvider.Instantiate("GameFieldPrefab");
            _gameFieldController = _gameFieldPrefab.GetComponent<GameFieldController>();
            _gameFieldController.Init(_userDataService);
            _gameFieldController.HoleController.Init(_userDataService, true);
            _gameFieldController.SetAllBonusBuildings();

            var cameraAdjuster = _screen.GetCameraAdjuster();
            cameraAdjuster.Initial(_gameFieldController.GameFieldBoxCollider);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<BonusGameScreen>(ConstScreens.BonusGameScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
            _screen.SetBalance(0);
            _ballRecoverySystem = _screen.GetBallRecoverySystem();
            _ballRecoverySystem.Init(_userDataService.GetUserData().BallRecoveryData);
            _ballRecoverySystem.StartBonusGame();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MenuStateController>();
            _gameFieldController.HoleController.OnBallEnter += BallOnHole;
            foreach (var buildingController in _gameFieldController.BuildingControllers)
            {
                buildingController.OnScoreFlyOut += AddBalance;
            }
            _screen.OnShootPressed += ShootButtonPressed;
            _screen.OnLeftPressed += LeftButtonPressed;
            _screen.OnRightPressed += RightButtonPressed;
        }

        private void UnsubscribeToEvents()
        {
            _gameFieldController.HoleController.OnBallEnter -= BallOnHole;
            _screen.OnShootPressed -= _gameFieldController.LauncherController.Launch;
            _screen.OnLeftPressed -= LeftButtonPressed;
            _screen.OnRightPressed -= RightButtonPressed;
            foreach (var buildingController in _gameFieldController.BuildingControllers)
            {
                buildingController.OnScoreFlyOut -= AddBalance;
            }
        }

        private void ShootButtonPressed()
        {
            if (_ballRecoverySystem.TryUseBall())
            {
                _gameFieldController.LauncherController.Launch();
            }
            else
            {
                Debug.Log("you havent a balls");
            }
        }

        private void LeftButtonPressed()
        {
            _gameFieldController.LauncherController.RotateLeft();
            _gameFieldController.LuckyWheelController.MoveLeft();
        }

        private void RightButtonPressed()
        {
            _gameFieldController.LauncherController.RotateRight();
            _gameFieldController.LuckyWheelController.MoveRight();
        }

        private void BallOnHole(int score)
        {
            _playerStatsData.TotalShotsFired++;
            AddBalance(score);

            if (!_ballRecoverySystem.HasBalls())
            {
                ShowResultPopup(_balance);
            }

            CheckScreen();
        }

        private void CheckScreen()
        {
            Debug.Log(_isBallOnField);
            if (_isBallOnField)
            {
                _screen.StartBonusGame();
                return;
            }

            if (_ballRecoverySystem.HasBalls())
            {
                _screen.EndBonusGame();
            }
            else
            {
                Debug.Log("Game Over");
            }
        }

        private void AddBalance(int score)
        {
            _balance += score;
            _screen.SetBalance(_balance);
        }

        private void ShowResultPopup(int score)
        {
            var popup = _uiService.GetPopup<GameOverPopup>(ConstPopups.GameOverPopup);

            _userInventory.BonusBalance = _userInventory.BonusBalance < _balance ? _balance : _userInventory.BonusBalance;

            var data = new GameOverPopupData() { Win = score };
            popup.Show(data);
            popup.OnContinuePressed += async () => await GoTo<GameplayStateController>();
        }

        private async Task ShowBonusGamePopup()
        {
            var popup = _uiService.GetPopup<BonusGamePopup>(ConstPopups.BonusGamePopup);
            popup.Show(null);
        }
    }
}