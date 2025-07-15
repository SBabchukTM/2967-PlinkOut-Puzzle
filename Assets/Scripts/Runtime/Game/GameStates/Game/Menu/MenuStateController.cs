using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using Runtime.Game.UI.Screen;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;
using Runtime.Game.GameStates.Game.Controllers;
using Runtime.Core.UI.Popup;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class MenuStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly UserDataService _userDataService;

        private MenuScreen _menuScreen;

        public MenuStateController(ILogger logger, IUiService uiService, UserDataService userDataService) : base(logger)
        {
            _uiService = uiService;
            _userDataService = userDataService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            UnsubscribeToEvents();
            await _uiService.HideScreen(ConstScreens.MenuScreen);
        }

        private void CreateScreen()
        {
            _menuScreen = _uiService.GetScreen<MenuScreen>(ConstScreens.MenuScreen);
            _menuScreen.Initialize();
            _menuScreen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _menuScreen.OnPlayPressed += async () => await GoTo<GameplayStateController>();
            _menuScreen.OnAccountPressed += async () => await GoTo<AccountStateController>();

            _menuScreen.OnLeaderboardPressed += ShowLeaderboardsPopup;
            _menuScreen.OnAchievementPressed += ShowAchievementPopup;
            _menuScreen.OnSettingsPressed += ShowSettingsPopup;
            _menuScreen.OnInfoPressed += ShowInfoPopup;
            _menuScreen.OnDailyRewardPressed += ShowDailyRewardPopup;
            //_menuScreen.OnPrivacyPressed += ShowPrivacyPolicyPopup;
            //_menuScreen.OnTermsPressed += ShowTermsPopup;
        }
        private void UnsubscribeToEvents()
        {
            _menuScreen.OnLeaderboardPressed -= ShowLeaderboardsPopup;
            _menuScreen.OnAchievementPressed += ShowAchievementPopup;
            _menuScreen.OnInfoPressed -= ShowInfoPopup;
            _menuScreen.OnSettingsPressed -= ShowSettingsPopup;
            _menuScreen.OnDailyRewardPressed -= ShowDailyRewardPopup;
            //_menuScreen.OnPrivacyPressed -= ShowPrivacyPolicyPopup;
            //_menuScreen.OnTermsPressed -= ShowTermsPopup;
        }

        private void ShowInfoPopup()
        {
            _uiService.ShowPopup(ConstPopups.InfoPopup);
        }

        private void ShowSettingsPopup()
        {
            _uiService.ShowPopup(ConstPopups.SettingsPopup);
        }


        private void ShowDailyRewardPopup()
        {
            _uiService.ShowPopup(ConstPopups.DailyRewardPopup);
        }

        private async void ShowLeaderboardsPopup()
        {
            var popup = _uiService.GetPopup<LeaderboardPopup>(ConstPopups.LeaderboardPopup);
            await popup.Initialize();
            await popup.Show(null);
        }

        private async void ShowAchievementPopup()
        {
            var popup = _uiService.GetPopup<AchievementPopup>(ConstPopups.AchievementPopup);
            await popup.Initialize();
            await popup.Show(null);
        }
    }
}