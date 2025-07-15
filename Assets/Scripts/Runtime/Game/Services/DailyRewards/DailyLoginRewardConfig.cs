using Runtime.Core.Infrastructure.SettingsProvider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyLoginRewardConfig", menuName = "Config/DailyLoginRewardConfig")]
public class DailyLoginRewardConfig : BaseSettings   
{
    public List<int> CoinRewards = new ();
    public Sprite CollectedRewardSprite;
    public Sprite AvailableToCollectRewardSprite;
    public Sprite LockedRewardSprite;
}
