using Runtime.Core.Data;
using UnityEngine;

namespace Runtime.Game.Services.AchievementSystem
{
    [CreateAssetMenu(fileName = "AchievementConfig", menuName = "Config/AchievementConfig")]
    public class AchievementConfig : ScriptableObject
    {
        [SerializeField] private AchievementType _achievementType;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private int _reward;

        public AchievementType AchievementType => _achievementType;
        public string Name => _name;
        public string Description => _description;
        public int Reward => _reward;
    }
}