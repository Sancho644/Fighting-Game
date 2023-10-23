using System;
using Audio;
using Infrastructure.Services.Randomizer;
using Logic;
using UnityEngine;
using Utils;

namespace Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private PlaySoundsComponent _playSounds;
        [SerializeField] private float _current;
        [SerializeField] private float _max;

        private IRandomService _randomService;
        private AudioClipsUtils _audioUtils;

        public event Action HealthChanged;

        public float Current
        {
            get => _current;
            set => _current = value;
        }

        public float Max
        {
            get => _max;
            set => _max = value;
        }

        public void Construct(IRandomService randomService)
        {
            _randomService = randomService;

            InitAudioUtils();
        }

        public void TakeDamage(float damage)
        {
            if (_current <= 0)
                return;

            _playSounds.PlayOneShot(_audioUtils.RandomizePunchClip());

            _current -= damage;
            _animator.PlayHit();

            HealthChanged?.Invoke();
        }

        private void InitAudioUtils()
        {
            _audioUtils = new AudioClipsUtils(_randomService);
            _audioUtils.AddPunchAudioClips(_playSounds, ClipId.Punch);
        }
    }
}