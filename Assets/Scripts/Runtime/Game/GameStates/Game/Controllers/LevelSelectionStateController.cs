using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Game.UI.Screen;
using Runtime.Game.Services.SettingsProvider;
using Runtime.Game.GameStates.Game.Menu;

namespace Runtime.Game.GameStates.Game.Controllers
{
    public class LevelSelectionStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly IAssetProvider _assetProvider;
        private readonly UserDataService _userDataService;
        private readonly GameplayStateController _gameplayStateController;

        private LevelSelectionScreen _screen;
        private GameLevelsConfig _gameLevelsConfig;

        public LevelSelectionStateController(ILogger logger, IUiService uiService, IAssetProvider assetProvider, UserDataService userDataService, GameplayStateController gameplayStateController) : base(logger)
        {
            _uiService = uiService;
            _assetProvider = assetProvider;
            _userDataService = userDataService;
            _gameplayStateController = gameplayStateController;
        }

        public override async UniTask Enter(CancellationToken cancellationToken)
        {
            await Initialize();
            CreateScreen();
            SubscribeToEvents();
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.LevelSelectionScreen);
        }

        private async UniTask Initialize()
        {
            _gameLevelsConfig = await _assetProvider.Load<GameLevelsConfig>(ConstConfigs.GameLevelsConfig);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<LevelSelectionScreen>(ConstScreens.LevelSelectionScreen);
            // _screen.Initialize(_gameLevelsConfig, _userDataService.GetUserData().UserInventory.Balance);
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MenuStateController>();
            _screen.OnPlayPressed += async (level) =>
            {
                _gameplayStateController.SetLevel(level);
                await GoTo<GameplayStateController>();
            };
        }
    }
}