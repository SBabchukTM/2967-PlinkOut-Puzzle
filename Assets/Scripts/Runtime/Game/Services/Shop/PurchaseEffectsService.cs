using System.Threading;
using Runtime.Core.Audio;
using Runtime.Game.Services.Audio;
using Runtime.Game.ShopSystem;

namespace Runtime.Game.Services.Shop
{
    public class PurchaseEffectsService : ISetShopSetup
    {
        private readonly IAudioService _audioService;
        
        private ShopSetup _shopItemStateConfig;

        public PurchaseEffectsService(IAudioService audioService) =>
                _audioService = audioService;

        public void SetShopSetup(ShopSetup shopSetup) =>
                _shopItemStateConfig = shopSetup;
        
        public void PlayFailedPurchaseAttemptEffect(ShopItemDisplayView shopItemDisplayView, CancellationToken cancellationToken)
        {
            if (_shopItemStateConfig.PurchaseEffectSettings.ShakeIfNotEnoughCurrency)
            {
                shopItemDisplayView
                        .Shake(cancellationToken, _shopItemStateConfig.PurchaseEffectSettings.PurchaseFailedShakeParameters)
                        .Forget();
            }

            if (_shopItemStateConfig.PurchaseEffectSettings.PlaySoundIfNotEnoughCurrency)
                _audioService.PlaySound(ConstAudio.ErrorSound);
        }
    }
}