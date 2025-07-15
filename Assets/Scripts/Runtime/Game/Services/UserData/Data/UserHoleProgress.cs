using System;
using System.Collections.Generic;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class UserHoleProgress
    {
        public List<int> HoleLevels = new();

        public List<bool> ActivatedSymbols = new();

        public int GetHoleLevel(int id)
        {
            if (id >= HoleLevels.Count)
            {
                HoleLevels.Add(0);
            }

            return HoleLevels[id];
        }

        public bool GetActivatedSymbol(int id)
        {
            if (id >= ActivatedSymbols.Count)
            {
                ActivatedSymbols.Add(false);
            }

            return ActivatedSymbols[id];
        }
    }
}