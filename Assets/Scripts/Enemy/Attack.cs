using System.Linq;
using Logic;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        private const string PlayerLayer = "Player";

        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private float _attackCooldown = 3f;

        public float EffectiveDistance = 0.5f;
        public float Cleavage = 1f;
        public float Damage = 10f;

        private Transform _heroTransform;
        private float _cooldown;
        private bool _isAttaking;
        private int _layerMask;
        private Collider[] _hits = new Collider[1];

        private bool _attackIsActive;

        public void Construct(Transform heroTransform) =>
            _heroTransform = heroTransform;

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer(PlayerLayer);
        }

        private void Update()
        {
            UpdateCooldown();

            if (CanAttack())
                StartAttack();
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(StartPoint(), Cleavage, 1);
                hit.transform.GetComponent<IHealth>().TakeDamage(Damage);
            }
        }

        private void OnAttackEnded()
        {
            _cooldown = _attackCooldown;
            _isAttaking = false;
        }

        public void DisableAttack() =>
            _attackIsActive = false;

        public void EnableAttack() =>
            _attackIsActive = true;

        private bool Hit(out Collider hit)
        {
            int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

            hit = _hits.FirstOrDefault();

            return hitsCount > 0;
        }

        private Vector3 StartPoint()
        {
            return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) +
                   transform.forward * EffectiveDistance;
        }

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            _animator.PlayAttack();

            _isAttaking = true;
        }

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _cooldown -= Time.deltaTime;
        }

        private bool CanAttack()
        {
            return _attackIsActive && !_isAttaking && CooldownIsUp();
        }

        private bool CooldownIsUp() =>
            _cooldown <= 0;
    }
}