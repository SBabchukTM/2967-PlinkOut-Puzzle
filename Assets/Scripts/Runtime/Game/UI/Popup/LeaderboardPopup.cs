using Cysharp.Threading.Tasks;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using Runtime.Game.UI.Screen;
using Runtime.Game.UI;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;
using Runtime.Core.UI.Data;
using System.Linq;
using UserProfile;

namespace Runtime.Core.UI.Popup
{
    public class LeaderboardPopup : BasePopup
    {
        private IAssetProvider _assetProvider;
        private GameObjectFactory _factory;
        private UserDataService _userDataService;
        private GameObject _leaderboardRecordDisplayPrefab;

        [SerializeField] private SimpleButton _closeButton;
        [SerializeField] private RectTransform _recordsParent;

        [Inject]
        public void Construct(
            IAssetProvider assetProvider,
            GameObjectFactory factory,
            UserDataService userDataService)
        {
            _assetProvider = assetProvider;
            _userDataService = userDataService;
            _factory = factory;
        }

        public async UniTask Initialize()
        {
            _leaderboardRecordDisplayPrefab = await _assetProvider.Load<GameObject>(ConstPrefabs.LeaderboardRecordPrefab);
        }

        public override UniTask Show(BasePopupData data, CancellationToken cancellationToken = default)
        {
            SubscribeToEvents();
            InitializePopup();
            return base.Show(data, cancellationToken);
        }

        public override void DestroyPopup()
        {
            UnsubscribeToEvents();
            base.DestroyPopup();
        }

        private void InitializePopup()
        {
            var records = CreateRecordsList();
            ParentRecords(records);
        }

        private void ParentRecords(List<LeaderboardRecordDisplay> records)
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
        }

        private List<LeaderboardRecordDisplay> CreateRecordsList()
        {
            var recordsDataList = CreateRecordsDataList();
            List<LeaderboardRecordDisplay> result = new(recordsDataList.Count);

            for (int i = 0; i < recordsDataList.Count; i++)
            {
                LeaderboardRecordDisplay display = _factory.Create<LeaderboardRecordDisplay>(_leaderboardRecordDisplayPrefab);
                display.Initialize(recordsDataList[i]);
                result.Add(display);
            }

            return result;
        }

        private List<LeaderboardRecord> CreateRecordsDataList()
        {
            var recordsData = CreateRecords();
            recordsData.Add(new LeaderboardRecord() { Name = UserProfileStorage.UserName, Balance = _userDataService.GetUserData().UserInventory.BonusBalance });

            recordsData = recordsData.OrderByDescending(x => x.Balance).ToList();

            for (int i = 0; i < recordsData.Count; i++)
                recordsData[i].Place = i + 1;

            return recordsData;
        }

        private List<LeaderboardRecord> CreateRecords() => new()
        {
            new LeaderboardRecord { Name = "Alex", Balance = 298500 },
            new LeaderboardRecord { Name = "Emma", Balance = 267000 },
            new LeaderboardRecord { Name = "Leo", Balance = 259500 },
            new LeaderboardRecord { Name = "Sophia", Balance = 243500 },
            new LeaderboardRecord { Name = "Liam", Balance = 229000 },
            new LeaderboardRecord { Name = "Mia", Balance = 213500 },
            new LeaderboardRecord { Name = "Noah", Balance = 199000 },
            new LeaderboardRecord { Name = "Ava", Balance = 186500 },
            new LeaderboardRecord { Name = "Ethan", Balance = 171000 },
            new LeaderboardRecord { Name = "Isabella", Balance = 159000 },
            new LeaderboardRecord { Name = "Lucas", Balance = 143500 },
            new LeaderboardRecord { Name = "Aria", Balance = 128000 },
            new LeaderboardRecord { Name = "Mason", Balance = 115000 },
            new LeaderboardRecord { Name = "Ella", Balance = 99500 },
            new LeaderboardRecord { Name = "Logan", Balance = 83500 },
            new LeaderboardRecord { Name = "Chloe", Balance = 67500 },
            new LeaderboardRecord { Name = "James", Balance = 52500 },
            new LeaderboardRecord { Name = "Zoe", Balance = 36500 },
        };
    }

    public class LeaderboardRecord
    {
        public string Name;
        public int Place;
        public int Balance;
    }
}