using Data;
using Infrastructure.Services.Input;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hero
{
    public class HeroMove : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _movementSpeed;

        private IInputService _inputService;
        private Camera _camera;
        
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (_inputService == null)
                return;
            
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                movementVector = _camera.transform.TransformDirection(_inputService.Axis);
                movementVector.y = 0;
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;

            _characterController.Move(_movementSpeed * movementVector * Time.deltaTime);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), _characterController.transform.position.AsVectorData());
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
            {
                Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
                if (savedPosition != null)
                    Warp(to: savedPosition);
            }
        }

        private string CurrentLevel() =>
            SceneManager.GetActiveScene().name;

        private void Warp(Vector3Data to)
        {
            _characterController.enabled = false;
            _characterController.transform.position = to.AsUnityVector();
            _characterController.enabled = true;
        }
    }
}