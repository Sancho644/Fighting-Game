using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class AgentMoveToPlayer : MonoBehaviour
    {
        private const float MinimalDistance = 1;

        [SerializeField] private NavMeshAgent _agent;

        private Transform _heroTransform;

        public void Construct(Transform heroTransform) => 
            _heroTransform = heroTransform;

        private void Update() => 
            SetDestinationForAgent();

        private void SetDestinationForAgent()
        {
            if (HeroNotReached())
                _agent.destination = _heroTransform.position;
        }

        private bool HeroNotReached()
        {
            return Vector3.Distance(_agent.transform.position, _heroTransform.position) >= MinimalDistance;
        }
    }
}