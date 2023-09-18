using Infrastructure.Factory;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class AgentMoveToPlayer : MonoBehaviour
    {
        private const float MinimalDistance = 1;

        [SerializeField] private NavMeshAgent _agent;

        private Transform _heroTransform;
        private IGameFactory _gameFactory;

        private void Start()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();

            if (_gameFactory.HeroGameObject != null)
                InitializeHeroTransform();
            else
                _gameFactory.HeroCreated += HeroCreated;
        }

        private void Update()
        {
            if (Initialized() && HeroNotReached())
                _agent.destination = _heroTransform.position;
        }

        private bool Initialized() =>
            _heroTransform != null;

        private void HeroCreated() =>
            InitializeHeroTransform();

        private void InitializeHeroTransform() =>
            _heroTransform = _gameFactory.HeroGameObject.transform;

        private bool HeroNotReached()
        {
            return Vector3.Distance(_agent.transform.position, _heroTransform.position) >= MinimalDistance;
        }
    }
}