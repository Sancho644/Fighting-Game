using Infrastructure.Services;
using Infrastructure.States;
using UnityEngine;

namespace Logic
{
    public class LevelTransferTrigger : MonoBehaviour
    {
        private const string PlayerTag = "Player";
        
        [SerializeField] private string _transferTo;

        private IGameStateMachine _stateMachine;

        private bool _triggered;

        private void Awake() =>
            _stateMachine = AllServices.Container.Single<IGameStateMachine>();

        private void OnTriggerEnter(Collider other)
        {
            if (_triggered)
                return;
            
            if (other.CompareTag(PlayerTag))
            {
                _stateMachine.Enter<LoadLevelState, string>(_transferTo);
                _triggered = true;
            }
        }
    }
}