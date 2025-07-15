using Runtime.Core.Data;
using UnityEngine;

namespace Runtime.Game.Services.AchievementSystem
{
    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "Config/BuildingConfig")]
    public class BuildingConfig : ScriptableObject
    {
        [SerializeField] private int _level;
        [SerializeField] private Sprite _buildingSprite;

        public int Level => _level;
        public Sprite BuildingSprite => _buildingSprite;
    }
}