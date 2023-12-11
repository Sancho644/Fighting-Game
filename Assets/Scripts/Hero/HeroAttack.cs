using Audio;
using Data;
using Infrastructure.Services.Input;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.Randomizer;
using Logic;
using UnityEngine;
using Utils;

namespace Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        private const string HittableTag = "Hittable";

        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private PlaySoundsComponent _playSounds;

        private IInputService _input;
        private IRandomService _randomService;
        private AudioClipsUtils _audioUtils;

        private Stats _stats;

        private int _layerMask;
        private float _radius;
        private Collider[] _hits = new Collider[1];

        public void Construct(IInputService input)
        {
            _input = input;
        }

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer(HittableTag);
        }

        private void Update()
        {
            if (_input == null)
                return;

            if (_input.IsAttackButtonUp() && !_animator.IsAttacking)
                _animator.PlayAttack();
        }

        private void OnAttack()
        {
            for (int i = 0; i < Hit(); i++)
            {
                if (TryGetHealth(i, out var health))
                {
                    health.TakeDamage(_stats.Damage);
                }
            }
        }

        private void OnHit()
        {
            for (int i = 0; i < Hit(); i++)
            {
                if (!TryGetHealth(i, out var health))
                {
                    _playSounds.Play(ClipId.Hit);
                    Debug.Log("tut");
                }
            }
        }

        private void OnAttackEnded()
        {
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _stats = progress.HeroStats;
        }

        private bool TryGetHealth(int i, out IHealth health)
        {
            return _hits[i].transform.parent.TryGetComponent<IHealth>(out health);
        }

        private int Hit() =>
            Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.DamageRadius, _hits, _layerMask);

        private Vector3 StartPoint() =>
            new Vector3(transform.position.x, _characterController.center.y / 2, transform.position.z);
    }
}