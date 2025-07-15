using Cysharp.Threading.Tasks.Triggers;
using Runtime.Game.Services.UserData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFieldController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoxCollider2D _gameFieldBoxCollider;
    [SerializeField] private HoleController _holeController;
    [SerializeField] private LauncherController _launcherController;
    [SerializeField] private List<BuildingController> _buildingControllers;
    [SerializeField] private LuckyWheelController _luckyWheelController;

    private UserDataService _userDataService;

    public HoleController HoleController => _holeController;
    public BoxCollider2D GameFieldBoxCollider => _gameFieldBoxCollider;
    public LauncherController LauncherController => _launcherController;
    public List<BuildingController> BuildingControllers => _buildingControllers;
    public LuckyWheelController LuckyWheelController => _luckyWheelController;

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Init(UserDataService userDataService)
    {
        _userDataService = userDataService;

        _luckyWheelController.Init(_userDataService);

        foreach (var controller in _buildingControllers)
        {
            controller.Init();
        }
        RefreshAllBuildings();
    }

    public void RefreshAllBuildings()
    {
        foreach (var controller in _buildingControllers)
        {
            var level = _userDataService.GetUserData().BuildingUpgradeData.BuildingDatas.Find(x => x.BuildingType == controller.BuildingType).level;
            controller.SetBuildingLevel(level);
        }
    }
    public void SetAllBonusBuildings()
    {
        foreach (var controller in _buildingControllers)
        {
            var level = 5;
            controller.SetBuildingLevel(level);
        }
    }
}