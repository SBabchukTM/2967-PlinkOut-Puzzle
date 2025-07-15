using Cysharp.Threading.Tasks;
using Runtime.Core.Data;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Core.UI.Data;
using Runtime.Game;
using Runtime.Game.Services.AchievementSystem;
using Runtime.Game.Services.SettingsProvider;
using Runtime.Game.Services.UserData;
using Runtime.Game.UI;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Zenject;

namespace Runtime.Core.UI.Popup
{
    public class AchievementPopup : BasePopup
    {
        private IAssetProvider _assetProvider;
        private GameObjectFactory _factory;
        private UserDataService _userDataService;
        private GameObject _achievementRecordDisplayPrefab;
        private AchievementsSetup _achievementsSetup;
        private AchievementController _achievementController;

        [SerializeField] private SimpleButton _closeButton;
        [SerializeField] private RectTransform _recordsParent;
        [SerializeField] private List<AchievementRecord> _records;
        [SerializeField] private List<AchievementRecordDisplay> _displays;


        [Inject]
        public void Construct(
            IAssetProvider assetProvider,
            GameObjectFactory factory,
            UserDataService userDataService,
            AchievementController achievementController)
        {
            _assetProvider = assetProvider;
            _userDataService = userDataService;
            _factory = factory;
            _achievementController = achievementController;
        }

        public async UniTask Initialize()
        {
            _achievementRecordDisplayPrefab = await _assetProvider.Load<GameObject>(ConstPrefabs.AchievementRecordPrefab);
            _achievementsSetup = await _assetProvider.Load<AchievementsSetup>(ConstConfigs.AchievementsSetup);
        }

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            InitializePopup();
            SubscribeToEvents();
            RefreshAllRecords();
            return base.Show(data, cancellationToken);
        }

        public override void DestroyPopup()
        {
            UnsubscribeToEvents();
            base.DestroyPopup();
        }

        private void InitializePopup()
        {
            foreach (Transform child in _recordsParent)
            {
                Destroy(child.gameObject);
            }

            _displays.Clear();

            var recordsDataList = CreateRecords();

            foreach (var record in recordsDataList)
            {
                var display = _factory.Create<AchievementRecordDisplay>(_achievementRecordDisplayPrefab);
                display.Initialize(record);
                display.transform.SetParent(_recordsParent, false);

                display.OnClaimButtonPressed += OnRewardClaimed;

                _displays.Add(display);
            }

            RefreshAllRecords();
        }

        private List<AchievementRecord> CreateRecords()
        {
            var records = new List<AchievementRecord>();

            foreach (var config in _achievementsSetup.AchievementConfigs)
            {
                var progress = GetOrCreateProgress(config.AchievementType);
                var record = new AchievementRecord()
                {
                    Progress = progress,
                    Config = config
                };
                records.Add(record);
            }
            return records;
        }

        private AchievementProgress GetOrCreateProgress(AchievementType type)
        {
            var userData = _userDataService.GetUserData();

            if (userData.AchievementData == null)
                userData.AchievementData = new AchievementData();

            var achievementData = userData.AchievementData;

            if (achievementData.achievements == null)
                achievementData.achievements = new List<AchievementProgress>();

            var progress = achievementData.achievements.Find(x => x.achievementType == type);

            if (progress == null)
            {
                progress = new AchievementProgress(type, AchievementStatus.InProgress);
                achievementData.achievements.Add(progress);
            }

            return progress;
        }

        private List<AchievementRecordDisplay> CreateRecordsList()
        {
            var recordsDataList = CreateRecords();
            List<AchievementRecordDisplay> result = new(recordsDataList.Count);

            for (int i = 0; i < recordsDataList.Count; i++)
            {
                var display = _factory.Create<AchievementRecordDisplay>(_achievementRecordDisplayPrefab);
                display.Initialize(recordsDataList[i]);
                result.Add(display);
            }

            return result;
        }

        private void ParentRecords(List<AchievementRecordDisplay> records)
        {
            foreach (var record in records)
                record.transform.SetParent(_recordsParent, false);
        }

        private void SubscribeToEvents()
        {
            _closeButton.Button.onClick.AddListener(DestroyPopup);
        }

        private void UnsubscribeToEvents()
        {
            _closeButton.Button.onClick.RemoveAllListeners();

            foreach (var display in _displays)
            {
                display.OnClaimButtonPressed -= OnRewardClaimed;
            }
        }

        private void OnRewardClaimed(AchievementType achievementType, int reward)
        {
            Debug.Log(achievementType + reward);
            _userDataService.GetUserData().UserInventory.Balance += reward;
            _achievementController.ClaimedAchievement(achievementType);
            RefreshAllRecords();
        }

        private void RefreshAllRecords()
        {
            _achievementController.CheckAllAchievements();
            foreach (var display in _displays)
            {
                display.RefreshDisplay();
            }
        }
    }

    public class AchievementRecord
    {
        public AchievementProgress Progress;
        public AchievementConfig Config;
    }
}