using System;
using System.Collections.Generic;
using Application.Services.UserData;
using Runtime.Application.BuildingData;
using Runtime.Application.UserAccountSystem;
using Runtime.Core.Data;
using Runtime.Game.Services.UserData.Data;

namespace Runtime.Game.Services.UserData
{
    [Serializable]
    public class UserData
    {
        public List<GameSessionData> GameSessionData = new List<GameSessionData>();
        public SettingsData SettingsData = new SettingsData();
        public GameData GameData = new GameData();
        public AchievementData AchievementData = new AchievementData();
        public UserInventory UserInventory = new UserInventory();
        public UserAccountData UserAccountData = new UserAccountData();
        public BuildingUpgradeData BuildingUpgradeData = new BuildingUpgradeData();
        public UserLoginData UserLoginData = new UserLoginData();
        public UserHoleProgress UserHoleProgress = new UserHoleProgress();
        public PlayerStatsData PlayerStatsData = new PlayerStatsData();
        public BallRecoveryData BallRecoveryData = new BallRecoveryData();
    }
}