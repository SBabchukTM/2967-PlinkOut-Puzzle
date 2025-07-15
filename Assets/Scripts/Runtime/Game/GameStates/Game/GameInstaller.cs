using Runtime.Application.UserAccountSystem;
using Runtime.Core.GameStateMachine;
using Runtime.Game.GameStates.Game.Controllers;
using Runtime.Game.GameStates.Game.Menu;
using Runtime.Game.Services.DailyRewardsFactory;
using UnityEngine;
using Zenject;

namespace Runtime.Game.GameStates.Game
{
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "Installers/GameInstaller")]
    public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<MenuStateController>().AsSingle();
            Container.Bind<GameplayStateController>().AsSingle();
            Container.Bind<BonusGameStateController>().AsSingle();
            Container.Bind<AccountStateController>().AsSingle();
            Container.Bind<LevelSelectionStateController>().AsSingle();
            Container.Bind<UserAccountService>().AsSingle();
            Container.Bind<ImageProcessingService>().AsSingle();
            Container.Bind<AvatarSelectionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<DailyRewardsFactory>().AsSingle();
            Container.Bind<AchievementController>().AsSingle();
        }
    }
}