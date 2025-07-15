using System;
using System.Collections.Generic;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class PlayerStatsData
    {
        public int TotalShotsFired = 0;
        public int LuckyWheelSpins = 0;
        public int BonusGamesPlayed = 0;
        public List<int> BuildingsUpgraded = new List<int>() { 0 };
    }
}