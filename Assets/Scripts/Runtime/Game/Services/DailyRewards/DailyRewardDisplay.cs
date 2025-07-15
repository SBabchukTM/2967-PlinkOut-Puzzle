using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Zenject.Internal;

namespace Runtime.Game.Services
{
    [Preserve]
    public class DailyRewardDisplay : MonoBehaviour
    {
        [SerializeField] private Button _collectButton;
        [SerializeField] private GameObject _collectedGO;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private TextMeshProUGUI _dayNumText;

        public event Action<DailyRewardDisplay, int> OnCollected;

        public void Initialize(bool collected, bool canCollect, int reward)
        {
            _collectedGO.SetActive(collected);
            _collectButton.gameObject.SetActive(canCollect);
            _rewardText.text = reward.ToString();
            _collectButton.onClick.AddListener(() => OnCollected?.Invoke(this, reward));
        }

        public void ClaimReward()
        {
            _collectedGO.SetActive(true);
            _collectButton.gameObject.SetActive(false);
        }
    }
}