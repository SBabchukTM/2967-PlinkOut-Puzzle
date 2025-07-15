using System;
using System.Collections.Generic;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class UserInventory
    {
        public int Balance = 0;
        public int BonusBalance = 0;
        public int LuckyBar = 0;
        public int UsedGameItemID;
        public List<int> PurchasedGameItemsIDs = new() { 0 };
    }
}