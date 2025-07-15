using Runtime.Application.BuildingData;
using Runtime.Game.Services.AchievementSystem;
using Runtime.Game.Services.UserData;
using Runtime.Game.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUpgradeMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuRoot;
    [SerializeField] private SimpleButton _closeButton;
    [SerializeField] private GameObject _buildingPrefab;
    [SerializeField] private List<BuildingSetup> _buildingSetups;
    [SerializeField] private List<BuildingUpgradeDisplay> _buildingViews;
    [SerializeField] private Transform _content;
    //  [SerializeField] private BuildingUpgradeData _buildingUpgradeData;
    private UserDataService _userDataService;

    public event Action<BuildingType, int> OnUpdateBuilding;

    private void Awake()
    {
        if (_closeButton != null)
            _closeButton.Button.onClick.AddListener(Close);

        _menuRoot?.SetActive(false);
    }

    public void Open()
    {
        _menuRoot?.SetActive(true);

        RefreshAllBuildingsView();
    }

    private void RefreshAllBuildingsView()
    {
        foreach (var buildingView in _buildingViews)
        {
            buildingView.RefreshDisplay();
        }
    }

    public void Close()
    {
        _menuRoot?.SetActive(false);
    }

    public void Init(UserDataService userDataService)
    {
        _userDataService = userDataService;
        foreach (Transform child in _content)
        {
            Destroy(child.gameObject);
        }

        _buildingViews.Clear();
        foreach (var setup in _buildingSetups)
        {
            GameObject instance = Instantiate(_buildingPrefab, _content);
            var display = instance.GetComponent<BuildingUpgradeDisplay>();

            _buildingViews.Add(display);

            if (display != null)
            {
                var level = GetBuildingLevel(setup.BuildingType);

                display.Init(userDataService, setup, level);

                display.OnUpgrade += (buildingType, level) =>
                {
                    OnUpdateBuilding?.Invoke(buildingType, level);
                    BuildingUpdate();
                };
            }
            else
            {
                Debug.LogWarning("Building prefab is missing BuildingUpgradeDisplay component.");
            }
        }
    }

    private void BuildingUpdate()
    {
        var data = _userDataService.GetUserData().BuildingUpgradeData;
        foreach (var buildingView in _buildingViews)
        {
            var level = buildingView.GetCurrentLevel();
            var type = buildingView.GetBuildingType();

            var record = data.BuildingDatas.Find(x => x.BuildingType == type);
            record.level = level;
        }
        _userDataService.SaveUserData();
        RefreshAllBuildingsView();
    }

    private int GetBuildingLevel(BuildingType type)
    {
        var data = _userDataService.GetUserData().BuildingUpgradeData;
        if (data == null)
            return 0;

        if (data.BuildingDatas == null)
            data.BuildingDatas = new List<BuildingData>();

        var building = data.BuildingDatas.Find(b => b.BuildingType == type);
        if (building != null)
        {
            return building.level;
        }
        else
        {
            var newBuildingData = new BuildingData
            {
                BuildingType = type,
                level = 1
            };
            data.BuildingDatas.Add(newBuildingData);
            return newBuildingData.level;
        }
    }
}