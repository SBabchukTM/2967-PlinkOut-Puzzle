using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Game.GameStates.Game.Controllers;
using Runtime.Game.GameStates.Game.Menu;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game
{
    public class GameState : StateController
    {
        private readonly StateMachine _stateMachine;

        private readonly MenuStateController _menuStateController;
        private readonly GameplayStateController _gameplayStateController;
        private readonly BonusGameStateController _bonusGameStateController;
        private readonly AccountStateController _accountStateController;
        private readonly LevelSelectionStateController _levelSelectionStateController;
        private readonly UserDataStateChangeController _userDataStateChangeController;

        public GameState(ILogger logger,
            MenuStateController menuStateController,
            GameplayStateController gameplayStateController,
            BonusGameStateController bonusGameStateController,
            AccountStateController accountStateController,
            LevelSelectionStateController levelSelectionStateController,
            StateMachine stateMachine,
            UserDataStateChangeController userDataStateChangeController) : base(logger)
        {
            _stateMachine = stateMachine;
            _menuStateController = menuStateController;
            _gameplayStateController = gameplayStateController;
            _bonusGameStateController = bonusGameStateController;
            _accountStateController = accountStateController;
            _levelSelectionStateController = levelSelectionStateController;
            _userDataStateChangeController = userDataStateChangeController;
        }

        public override async UniTask Enter(CancellationToken cancellationToken)
        {
            await _userDataStateChangeController.Run(default);

            _stateMachine.Initialize(
                _menuStateController,
                  _gameplayStateController,
                  _bonusGameStateController,
                  _accountStateController,
                  _levelSelectionStateController
                );
            _stateMachine.GoTo<MenuStateController>().Forget();
        }
    }
}