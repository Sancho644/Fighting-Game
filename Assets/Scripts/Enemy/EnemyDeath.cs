using System;
using System.Collections;
using Audio;
using Infrastructure.Services.Randomizer;
using UnityEngine;
using Utils;
using Utils.ObjectPool;

namespace Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator))]
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private Aggro _aggro;
        [SerializeField] private PlaySoundsComponent _playSounds;
        [SerializeField] private GameObject _deathFx;
        [SerializeField] private Collider _hurtBox;
        [SerializeField] private float _deathCooldown = 3f;

        private IRandomService _randomService;
        private AudioClipsUtils _audioUtils;

        public event Action Happened;

        public void Construct(IRandomService randomService)
        {
            _randomService = randomService;

            InitAudioUtils();
        }
        
        private void Start()
        {
            _health.HealthChanged += HealthChanged;
        }

        private void OnDestroy()
        {
            _health.HealthChanged -= HealthChanged;
        }

        private void HealthChanged()
        {
            if (_health.Current <= 0)
                Die();
        }

        private void Die()
        {
            _health.HealthChanged -= HealthChanged;

            _aggro.DisableAggro();
            _animator.PlayDeath();
            _playSounds.PlayOneShot(_audioUtils.RandomizePunchClip());
            _hurtBox.enabled = false;

            SpawnDeathFx();
            StartCoroutine(DestroyTimer());

            Happened?.Invoke();
        }

        private void SpawnDeathFx()
        {
            Pool.Instance.Get(_deathFx, transform.position);
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(_deathCooldown);

            Destroy(gameObject);
        }
        
        private void InitAudioUtils()
        {
            _audioUtils = new AudioClipsUtils(_randomService);
            _audioUtils.AddPunchAudioClips(_playSounds, ClipId.Finisher);
        }
    }
}