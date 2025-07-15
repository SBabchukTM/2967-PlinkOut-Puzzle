using Runtime.Core.Infrastructure.SettingsProvider;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Application.UserAccountSystem
{
    [CreateAssetMenu(fileName = "DefaultAvatarsConfig", menuName = "Config/DefaultAvatarsConfig")]
    public class DefaultAvatarsConfig : BaseSettings
    {
        [SerializeField] private List<Sprite> _avatars;
    
        public List<Sprite> Avatars => _avatars;
    }
}

