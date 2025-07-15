using System.Threading;
using Runtime.Core.Audio;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UI;
using Runtime.Game.ShopSystem;
using Runtime.Game.UI.Popup;
using Runtime.Game.UI.Popup.Data;

namespace Runtime.Game.Services.Shop
{
    public class ProcessPurchaseService : ISetShopSetup
    {
        /*  private readonly ShopService _shopService;
          private readonly PurchaseEffectsService _purchaseEffectsService;
          private readonly SelectPurchaseItemService _selectPurchaseItemService;
          private readonly IUiService _uiService;
          private readonly ShopItemsDisplayService _shopItemsDisplayService;
          private readonly IAudioService _audioService;

          private ShopSetup _shopSetup;

          public ProcessPurchaseService(ShopService shopService, PurchaseEffectsService purchaseEffectsService, 
                  SelectPurchaseItemService selectPurchaseItemService, IUiService uiService, 
                  ShopItemsDisplayService shopItemsDisplayService, IAudioService audioService)
          {
              _shopService = shopService;
              _purchaseEffectsService = purchaseEffectsService;
              _selectPurchaseItemService = selectPurchaseItemService;
              _uiService = uiService;
              _shopItemsDisplayService = shopItemsDisplayService;
              _audioService = audioService;
          }

          public void SetShopSetup(ShopSetup shopSetup) =>
                  _shopSetup = shopSetup;

          public void ProcessPurchaseAttempt(ShopItemDisplayView shopItemDisplayView, CancellationToken cancellationToken)
          {
              var shopItemModel = shopItemDisplayView.GetShopItemModel();

              switch (shopItemModel.ItemState)
              {
                  case ShopItemState.NotPurchased:
                      ProcessPurchase(shopItemDisplayView, cancellationToken);
                      break;
                  case ShopItemState.Purchased:
                      SelectItem(shopItemModel);
                      UpdateStatus();
                      break;
                  case ShopItemState.Selected:
                      UpdateStatus();
                      break;
              }
          }

          private async void ProcessPurchase(ShopItemDisplayView shopItemDisplayView, CancellationToken cancellationToken)
          {
              if(!_shopService.CanPurchaseItem(shopItemDisplayView.GetShopItemModel().ShopItem))
              {
                  _purchaseEffectsService.PlayFailedPurchaseAttemptEffect(shopItemDisplayView, cancellationToken);
                  return;
              }

              if (_shopSetup.ConfirmPurchase)
              {
                  var popup = await _uiService
                      .ShowPopup(ConstPopups.ItemPurchasePopup,
                          new ItemPurchasePopupData { ShopItem = shopItemDisplayView.GetShopItemModel().ShopItem },
                          cancellationToken) as ItemPurchasePopup;

                  Subscribe(shopItemDisplayView, popup);
              }
              else
                  AcceptPurchase(shopItemDisplayView);
          }

          private void Subscribe(ShopItemDisplayView shopItemDisplayView, ItemPurchasePopup popup)
          {
              popup.OnAcceptPressedEvent += () => { OnAcceptButtonPressed(shopItemDisplayView, popup); };
              popup.OnDenyPressedEvent += () => OnDenyButtonPressed(shopItemDisplayView, popup);
          }

          private void SelectItem(ShopItemDisplayModel shopDisplayModel)
          {
              PlaySound(ConstAudio.SelectSound, _shopSetup.PurchaseEffectSettings.PlaySoundOnSelectPurchased);
              _selectPurchaseItemService.SelectPurchasedItem(shopDisplayModel);
          }

          private void PlaySound(string sound, bool condition)
          {
              if (condition)
                  _audioService.PlaySound(sound);
          }

          private void UpdateStatus() =>
                  _shopItemsDisplayService.UpdateItemsStatus();

          private static void DestroyPopup(ItemPurchasePopup popup) =>
                  popup.DestroyPopup();

          private void OnDenyButtonPressed(ShopItemDisplayView shopItemDisplayView, ItemPurchasePopup popup)
          {
              popup.OnAcceptPressedEvent -= () => { OnAcceptButtonPressed(shopItemDisplayView, popup); };
              DestroyPopup(popup);
          }

          private void AcceptPurchase(ShopItemDisplayView shopItemDisplayView)
          {
              _shopService.PurchaseShopItem(shopItemDisplayView);

              SelectItem(shopItemDisplayView.GetShopItemModel());
              PlaySound(ConstAudio.PurchaseSound, condition: _shopSetup.PurchaseEffectSettings.PlaySoundOnPurchase);
              UpdateStatus();
          }

          private void OnAcceptButtonPressed(ShopItemDisplayView shopItemDisplayView, ItemPurchasePopup popup)
          {
              AcceptPurchase(shopItemDisplayView);
              DestroyPopup(popup);
          }*/
        public void SetShopSetup(ShopSetup shopSetup)
        {
            throw new System.NotImplementedException();
        }
    }
}