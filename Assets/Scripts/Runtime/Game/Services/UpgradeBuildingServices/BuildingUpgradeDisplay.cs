using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Runtime.Game.UI;
using Runtime.Game.Services.AchievementSystem;
using Runtime.Game.Services.UserData;

public class BuildingUpgradeDisplay : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _upgradeCostText;
    [SerializeField] private SimpleButton _upgradeButton;
    [SerializeField] private Slider _levelSlider;
    [SerializeField] private Image _image;

    [Header("Building Settings")]
    [SerializeField] private BuildingSetup _buildingSetup;
    [SerializeField] private int[] _upgradeCosts;
    [SerializeField] private int _maxLevel = 5;

    private UserDataService _userDataService;
    private int _currentLevel = 0;
    private int _playerCoins = 0;

    public event Action<BuildingType, int> OnUpgrade;

    private void OnEnable()
    {
        _upgradeButton.Button.onClick.AddListener(Upgrade);
    }

    private void OnDisable()
    {
        _upgradeButton.Button.onClick.RemoveAllListeners();
    }

    public void Init(UserDataService userDataService, BuildingSetup setup, int level)
    {
        _userDataService = userDataService;
        _buildingSetup = setup;
        SetCurrentLevel(level);
    }

    public void SetPlayerCoins(int coins)
    {
        _playerCoins = coins;
        RefreshDisplay();
    }

    public void SetCurrentLevel(int level)
    {
        _currentLevel = Mathf.Clamp(level, 0, _maxLevel);
        RefreshDisplay();
    }

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    public BuildingType GetBuildingType()
    {
        return _buildingSetup.BuildingType;
    }

    private void Upgrade()
    {
        if (_currentLevel >= _maxLevel) return;

        int cost = _upgradeCosts[_currentLevel - 1];

        if (_currentLevel < _maxLevel)
        {
            _currentLevel++;
            SetCurrentLevel(_currentLevel);
            _userDataService.GetUserData().UserInventory.Balance -= cost;
            _userDataService.SaveUserData();
            OnUpgrade?.Invoke(_buildingSetup.BuildingType, _currentLevel);
            RefreshDisplay();
        }
    }

    public void RefreshDisplay()
    {
        _levelText.text = $"{_currentLevel}/{_maxLevel}";
        _levelSlider.value = _currentLevel;

        if (_currentLevel >= _maxLevel)
        {
            _upgradeCostText.text = "Max Level";
            _upgradeButton.Button.interactable = false;
            _image.sprite = _buildingSetup.BuildingConfigs[_maxLevel - 1].BuildingSprite;
        }
        else
        {
            _image.sprite = _buildingSetup.BuildingConfigs[_currentLevel - 1].BuildingSprite;
            int nextCost = _upgradeCosts[_currentLevel - 1];
            _upgradeCostText.text = nextCost.ToString();
            var playerCoins = _userDataService.GetUserData().UserInventory.Balance;
            _upgradeButton.Button.interactable = playerCoins >= nextCost;
        }
    }
}