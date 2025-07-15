using Runtime.Core.UI.Popup;
using System;
using System.Collections.Generic;

namespace Runtime.Core.Data
{
    [Serializable]
    public class AchievementData
    {
        public List<AchievementProgress> achievements;
    }

    [Serializable]
    public class AchievementProgress
    {
        public AchievementType achievementType;
        public AchievementStatus achievementStatus;

        public AchievementProgress(AchievementType achievementType, AchievementStatus achievementStatus)
        {
            this.achievementType = achievementType;
            this.achievementStatus = achievementStatus;
        }
    }

    [Serializable]
    public enum AchievementStatus
    {
        InProgress,
        Completed,
        Claimed
    }

    [Serializable]
    public enum AchievementType
    {
        FirstShot,
        ShotMaster,
        WheelOfFortune,
        MasterBuilder,
        FirstBonus
    }
}