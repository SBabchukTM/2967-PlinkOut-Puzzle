using Runtime.Core.Data;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;
using System.Collections.Generic;
using System.Linq;

public class AchievementController
{
    private readonly UserDataService _userDataService;
    private PlayerStatsData _playerStatsData;

    public AchievementController(UserDataService userDataService)
    {
        _userDataService = userDataService;
        _playerStatsData = _userDataService.GetUserData().PlayerStatsData;
    }

    private AchievementData EnsureAchievementData()
    {
        var userData = _userDataService.GetUserData();

        if (userData.AchievementData == null)
            userData.AchievementData = new AchievementData();

        if (userData.AchievementData.achievements == null)
            userData.AchievementData.achievements = new List<AchievementProgress>();

        return userData.AchievementData;
    }

    public void CompleteAchievement(AchievementType type)
    {
        var data = EnsureAchievementData();

        var progress = data.achievements.Find(a => a.achievementType == type);
        if (progress == null)
        {
            progress = new AchievementProgress(type, AchievementStatus.Completed);
            data.achievements.Add(progress);
        }
        else if (progress.achievementStatus == AchievementStatus.InProgress)
        {
            progress.achievementStatus = AchievementStatus.Completed;
        }

        _userDataService.SaveUserData();
    }

    public void ClaimedAchievement(AchievementType type)
    {
        var data = EnsureAchievementData();

        var progress = data.achievements.Find(a => a.achievementType == type);
        if (progress != null && progress.achievementStatus == AchievementStatus.Completed)
        {
            progress.achievementStatus = AchievementStatus.Claimed;
        }

        _userDataService.SaveUserData();
    }

    public AchievementStatus GetStatus(AchievementType type)
    {
        var data = EnsureAchievementData();
        var progress = data.achievements.Find(a => a.achievementType == type);
        return progress?.achievementStatus ?? AchievementStatus.InProgress;
    }

    public void CheckAllAchievements()
    {
        _playerStatsData = _userDataService.GetUserData().PlayerStatsData;
        if (_playerStatsData.TotalShotsFired > 0)
        {
            CompleteAchievement(AchievementType.FirstShot);
        }

        if (_playerStatsData.TotalShotsFired > 50)
        {
            CompleteAchievement(AchievementType.ShotMaster);
        }

        if (_playerStatsData.LuckyWheelSpins > 0)
        {
            CompleteAchievement(AchievementType.WheelOfFortune);
        }

        if (_playerStatsData.BonusGamesPlayed > 0)
        {
            CompleteAchievement(AchievementType.FirstBonus);
        }

        if (_playerStatsData.BuildingsUpgraded.All(level => level > 4))
        {
            CompleteAchievement(AchievementType.MasterBuilder);
        }
    }
}