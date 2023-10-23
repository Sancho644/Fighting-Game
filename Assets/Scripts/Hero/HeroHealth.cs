using System;
using Audio;
using Data;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.Randomizer;
using Logic;
using UnityEngine;
using Utils;
using State = Data.State;

namespace Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
    {
        [SerializeField] private HeroAnimator _heroAnimator;
        [SerializeField] private PlaySoundsComponent _playSounds;

        private State _state;
        
        private IRandomService _randomService;
        private AudioClipsUtils _audioUtils;

        public event Action HealthChanged;

        public float Current
        {
            get => _state.CurrentHp;
            set
            {
                if (_state.CurrentHp != value)
                {
                    _state.CurrentHp = value;
                    
                    HealthChanged?.Invoke();
                }
            }
        }

        public float Max
        {
            get => _state.MaxHp;
            set => _state.MaxHp = value;
        }

        public void Construct(IRandomService randomService)
        {
            _randomService = randomService;

            InitAudioUtils();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _state = progress.HeroState;
            HealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroState.CurrentHp = Current;
            progress.HeroState.MaxHp = Max;
        }

        public void TakeDamage(float damage)
        {
            if (Current <= 0)
                return;

            _playSounds.PlayOneShot(_audioUtils.RandomizePunchClip());

            Current -= damage;
            _heroAnimator.PlayHit();
        }
        
        private void InitAudioUtils()
        {
            _audioUtils = new AudioClipsUtils(_randomService);
            _audioUtils.AddPunchAudioClips(_playSounds, ClipId.Punch);
        }
    }
}