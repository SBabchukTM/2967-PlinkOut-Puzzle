using UnityEngine;

namespace Runtime.Core.Audio
{
    public static class AudioDataExtensions
    {
        public static AudioClip GetClip(this AudioConfig config, string clipId)
        {
            var audioData = config.Audio.Find(x => x.Id == clipId);
            return audioData.Clip;
        }
    }
}