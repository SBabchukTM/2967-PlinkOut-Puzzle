using Runtime.Game.Services.AchievementSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Application.BuildingData
{
    [Serializable]
    public class BuildingUpgradeData
    {
        public List<BuildingData> BuildingDatas;
    }
    [Serializable]
    public class BuildingData
    {
        public BuildingType BuildingType;
        public int level;
    }
}