using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Core.UI.Data;
using Runtime.Core.UI.Popup;
using Runtime.Game.GameStates.Game.Menu;
using Runtime.Game.Services.AchievementSystem;
using Runtime.Game.Services.SettingsProvider;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;
using Runtime.Game.UI.Screen;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Controllers
{
    public class GameplayStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly IAudioService _audioService;
        private readonly UserDataService _userDataService;
        private readonly IAssetProvider _assetProvider;

        private GameplayScreen _screen;
        private UserInventory _userInventory;
        private PlayerStatsData _playerStatsData;

        private GameObject _gameFieldPrefab;
        private GameFieldController _gameFieldController;
        private BuildingUpgradeMenu _buildingUpgradeMenu;
        private BallRecoverySystem _ballRecoverySystem;

        private CancellationTokenSource _cts;

        private bool _gameEnded;
        private int _level;
        private int _balance;
        private bool _isBonusGame = false;
        private bool _isBallOnField = false;

        public GameplayStateController(ILogger logger, IUiService uiService, IAudioService audioService, IAssetProvider assetProvider,
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
            _balance = _userInventory.Balance;

            _gameEnded = false;

            CreateScreen();

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            _cts = new CancellationTokenSource();

            await InitGameFieldAsync(_cts.Token);
            SubscribeToEvents();
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
            await _uiService.HideScreen(ConstScreens.GameplayScreen);
        }

        public void SetLevel(int level) =>
                _level = level;

        private async Task InitGameFieldAsync(CancellationToken token)
        {
            _gameFieldPrefab = await _assetProvider.Instantiate("GameFieldPrefab");
            _gameFieldController = _gameFieldPrefab.GetComponent<GameFieldController>();
            _gameFieldController.Init(_userDataService);
            _gameFieldController.HoleController.Init(_userDataService, false);

            var cameraAdjuster = _screen.GetCameraAdjuster();
            cameraAdjuster.Initial(_gameFieldController.GameFieldBoxCollider);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<GameplayScreen>(ConstScreens.GameplayScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
            _screen.SetBalance(_userInventory.Balance);
            _buildingUpgradeMenu = _screen.GetBuildingUpgradeMenu();
            _buildingUpgradeMenu.Init(_userDataService);
            _ballRecoverySystem = _screen.GetBallRecoverySystem();
            _ballRecoverySystem.Init(_userDataService.GetUserData().BallRecoveryData);
        }

        private void SubscribeToEvents()
        {
            _buildingUpgradeMenu.OnUpdateBuilding += OnUpdateButtonPressed;
            _screen.OnBackPressed += async () => await GoTo<MenuStateController>();
            _gameFieldController.HoleController.OnBallEnter += BallOnHole;
            foreach (var buildingController in _gameFieldController.BuildingControllers)
            {
                buildingController.OnScoreFlyOut += AddBalance;
            }
            _screen.OnShootPressed += ShootButtonPressed;
            _screen.OnLeftPressed += LeftButtonPressed;
            _screen.OnRightPressed += RightButtonPressed;
            _ballRecoverySystem.OnBallsChanged += CheckScreen;
        }

        private void UnsubscribeToEvents()
        {
            _buildingUpgradeMenu.OnUpdateBuilding -= OnUpdateButtonPressed;
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

        private async void BallOnHole(int score)
        {
            _isBallOnField = false;
            _playerStatsData.TotalShotsFired++;
            AddBalance(score);

            if (_gameFieldController.LuckyWheelController.IsWheelFilled())
            {
                await ShowLuckyWheel();
            }
            CheckScreen();

            if (_gameFieldController.HoleController.AreAllSymbolsActivated())
            {
                _gameFieldController.HoleController.DeactivateAllSymbols();
                StartBonusGame();
            }
        }

        private void CheckScreen()
        {
            Debug.Log(_isBallOnField);
            if (_isBallOnField)
            {
                _screen.StartGame();
                return;
            }

            if (_ballRecoverySystem.HasBalls())
            {
                _screen.EndGame();
            }
            else
            {
                _screen.SetActiveUpgradeBuildingButton(true);
                _screen.SetActiveShootButton(false);
            }
        }

        private void AddBalance(int score)
        {
            _userInventory.Balance += score;
            _balance = _userInventory.Balance;
            _screen.SetBalance(_balance);
        }

        private async Task ShowLuckyWheel()
        {
            _playerStatsData.LuckyWheelSpins++;
            var popup = await _uiService.ShowPopup(ConstPopups.LuckyWheelPopup);
        }

        private async void StartBonusGame()
        {
            _playerStatsData.BonusGamesPlayed++;
            await GoTo<BonusGameStateController>();
        }

        private void OnUpdateButtonPressed(BuildingType buildingType, int level)
        {
            _screen.SetBalance(_userInventory.Balance);
            var buildingController = _gameFieldController.BuildingControllers.Find(x => x.BuildingType == buildingType);
            if (buildingController != null)
            {
                buildingController.SetBuildingLevel(level);
            }
            _playerStatsData.BuildingsUpgraded.Clear();
            foreach (var controller in _gameFieldController.BuildingControllers)
            {
                _playerStatsData.BuildingsUpgraded.Add(controller.Level);
            }

            Debug.Log("OnUpdateButtonPressed" + level);
        }
    }
}