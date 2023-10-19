using UnityEngine;

namespace Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private GameBootstrapper _bootstrapperPrefab;

        private void Awake()
        {
            var bootsrtapper = FindObjectOfType<GameBootstrapper>();

            if (bootsrtapper == null)
                Instantiate(_bootstrapperPrefab);
        }
    }
}