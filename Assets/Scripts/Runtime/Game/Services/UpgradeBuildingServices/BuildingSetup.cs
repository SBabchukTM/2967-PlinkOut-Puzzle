using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Game.Services.AchievementSystem
{
    [CreateAssetMenu(fileName = "BuildingSetup", menuName = "Config/BuildingSetup")]
    public class BuildingSetup : ScriptableObject
    {
        [SerializeField] private BuildingType _buildingType;
        [SerializeField] private List<BuildingConfig> _buildingConfigs;

        public BuildingType BuildingType => _buildingType;
        public List<BuildingConfig> BuildingConfigs => _buildingConfigs;
    }

    public enum BuildingType
    {
        Barn,
        Hay,
        House,
        Cock,
        Farmer,
        Mill,
        Well,
        Chickens
    }
}