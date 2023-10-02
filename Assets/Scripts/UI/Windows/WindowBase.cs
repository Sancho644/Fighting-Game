using Data;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        protected IPersistentProgressService ProgressService;
        protected PlayerProgress Progress => ProgressService.Progress;
        
        public Button CloseButton;

        protected void Construct(IPersistentProgressService progressService) => 
            ProgressService = progressService;

        private void Awake() =>
            OnAwake();

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy() =>
            Cleanup();

        protected virtual void OnAwake() =>
            CloseButton.onClick.AddListener(() => Destroy(gameObject));

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