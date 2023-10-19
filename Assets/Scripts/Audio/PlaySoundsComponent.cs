using UnityEngine;

namespace Audio
{
    public class PlaySoundsComponent : MonoBehaviour
    {
        [SerializeField] private ClipData[] _sounds;

        public ClipData[] Sounds => _sounds;

        private AudioSource _source;

        public void Construct(AudioSource source)
        {
            _source = source;
        }

        public void Play(ClipId clipId)
        {
            foreach (ClipData clipData in _sounds)
            {
                if (clipData.Id != clipId)
                    continue;

                _source.PlayOneShot(clipData.Clip);

                break;
            }
        }

        public void PlayOneShot(AudioClip clip)
        {
            _source.PlayOneShot(clip);
        }
    }
}