using System.Collections.Generic;
using Runtime.Core.Infrastructure.SettingsProvider;
using UnityEngine;

namespace Runtime.Game.ShopSystem
{
    [CreateAssetMenu(fileName = "ShopSetup", menuName = "Config/ShopSetup")]
    public class ShopSetup : BaseSettings
    {
        [SerializeField] private List<ShopItemStateConfig> _shopItemStateConfigs;
        [SerializeField] private List<ShopItem> _shopItems = new();
        [SerializeField, Space] private bool _confirmPurchase = true;
        [SerializeField, Space] private PurchaseEffectSettings _purchaseEffectSettings;
        
        public List<ShopItem> ShopItems => _shopItems;
        public bool ConfirmPurchase => _confirmPurchase;
        public PurchaseEffectSettings PurchaseEffectSettings => _purchaseEffectSettings;
        public List<ShopItemStateConfig> ShopItemStateConfigs => _shopItemStateConfigs;
        
        private void OnValidate()
        {
            HashSet<int> uniqueIDs = new();

            foreach (var item in _shopItems)
                if (!uniqueIDs.Add(item.ItemID))
                    Debug.LogError($"Shop item {item.name} does not have a unique ID!");
        }
    }
}