using System.Collections.Generic;
using Audio;
using Infrastructure.Services.Randomizer;
using UnityEngine;

namespace Utils
{
    public class AudioClipsUtils
    {
        private List<AudioClip> _punchSoundsList = new List<AudioClip>();

        private IRandomService _randomService;

        public AudioClipsUtils(IRandomService randomService)
        {
            _randomService = randomService;
        }

        public AudioClip RandomizePunchClip()
        {
            return _punchSoundsList[_randomService.Next(0, _punchSoundsList.Count)];
        }

        public void AddPunchAudioClips(PlaySoundsComponent playSounds, ClipId clipId)
        {
            foreach (ClipData clipData in playSounds.Sounds)
            {
                if (clipData.Id == clipId)
                {
                    _punchSoundsList.Add(clipData.Clip);
                }
            }
        }
    }
}