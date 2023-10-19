using System.Collections.Generic;
using Audio;
using Data;
using Infrastructure.Services.Input;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.Randomizer;
using Logic;
using UnityEngine;

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

        private Stats _stats;

        private int _layerMask;
        private float _radius;
        private Collider[] _hits = new Collider[1];
        private List<AudioClip> _punchSoundsList = new List<AudioClip>();

        public void Construct(IInputService input, IRandomService randomService)
        {
            _input = input;
            _randomService = randomService;
        }

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer(HittableTag);
            
            AddPunchAudioClips();
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
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);

                _playSounds.PlayOneShot(RandomizePunchClip());
                
                return;
            }
            
            _playSounds.Play(ClipId.Hit);
        }

        private void OnAttackEnded()
        {
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _stats = progress.HeroStats;
        }

        private AudioClip RandomizePunchClip()
        {
            return _punchSoundsList[_randomService.Next(0, _punchSoundsList.Count)];
        }

        private void AddPunchAudioClips()
        {
            foreach (ClipData clipData in _playSounds.Sounds)
            {
                if (clipData.Id == ClipId.Punch)
                {
                    _punchSoundsList.Add(clipData.Clip);
                }
            }
        }

        private int Hit() =>
            Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.DamageRadius, _hits, _layerMask);

        private Vector3 StartPoint() =>
            new Vector3(transform.position.x, _characterController.center.y / 2, transform.position.z);
    }
}