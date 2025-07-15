using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Services.ScreenOrientation;

namespace Runtime.Game.Services.SettingsProvider
{
    public class SettingsProvider : ISettingProvider
    {
        private readonly IAssetProvider _assetProvider;

        private Dictionary<Type, BaseSettings> _settings = new Dictionary<Type, BaseSettings>();

        public SettingsProvider(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask Initialize()
        {
            var screenOrientationConfig = await _assetProvider.Load<ScreenOrientationConfig>(ConstConfigs.ScreenOrientationConfig);
            var audioConfig = await _assetProvider.Load<AudioConfig>(ConstConfigs.AudioConfig);
            var dailyLoginRewardConfig = await _assetProvider.Load<DailyLoginRewardConfig>(ConstConfigs.DailyLoginRewardConfig);

            Set(screenOrientationConfig);
            Set(audioConfig);
            Set(dailyLoginRewardConfig);
        }

        public T Get<T>() where T : BaseSettings
        {
            if (_settings.ContainsKey(typeof(T)))
            {
                var setting = _settings[typeof(T)];
                return setting as T;
            }

            throw new Exception("No setting found");
        }

        public void Set(BaseSettings config)
        {
            if (_settings.ContainsKey(config.GetType()))
                return;

            _settings.Add(config.GetType(), config);
        }
    }
}