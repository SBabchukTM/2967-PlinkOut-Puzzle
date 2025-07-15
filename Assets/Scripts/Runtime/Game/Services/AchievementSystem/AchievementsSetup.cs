using Runtime.Core.Infrastructure.SettingsProvider;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Game.Services.AchievementSystem
{
    [CreateAssetMenu(fileName = "AchievementsSetup", menuName = "Config/AchievementsSetup")]
    public class AchievementsSetup : BaseSettings
    {
        [SerializeField] private List<AchievementConfig> _achievementConfigs;

        public List<AchievementConfig> AchievementConfigs => _achievementConfigs;
    }
}