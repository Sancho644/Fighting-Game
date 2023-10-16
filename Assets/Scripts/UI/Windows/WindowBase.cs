using Data;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        private IPersistentProgressService _progressService;
        protected PlayerProgress Progress => _progressService.Progress;
        

        protected void Construct(IPersistentProgressService progressService) => 
            _progressService = progressService;

        private void Awake() =>
            OnAwake();

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy() =>
            Cleanup();

        protected virtual void OnAwake()
        {
        }

        protected virtual void Initialize()
        {
        }

        protected virtual void SubscribeUpdates()
        {
        }

        protected virtual void Cleanup()
        {
        }
    }
}