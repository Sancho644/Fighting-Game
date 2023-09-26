using System.Collections;
using Data;
using TMPro;
using UnityEngine;

namespace Enemy
{
    public class LootPiece : MonoBehaviour
    {
        [SerializeField] private GameObject _lootPrefab;
        [SerializeField] private GameObject _pickupFxPrefab;
        [SerializeField] private GameObject _pickupPopup;
        [SerializeField] private TextMeshPro _lootText;
        [SerializeField] private float _destroyCooldown;

        private Loot _loot;
        private bool _picked;
        private WorldData _worldData;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
        }

        public void Initialize(Loot loot)
        {
            _loot = loot;
        }

        private void OnTriggerEnter(Collider other) =>
            Pickup();

        private void Pickup()
        {
            if (_picked)
                return;

            _picked = true;

            UpdateWorldData();
            HideLootPrefab();
            PlayPickupFx();
            ShowText();

            StartCoroutine(StartDestroyTimer());
        }

        private void UpdateWorldData()
        {
            _worldData.LootData.Collect(_loot);
        }

        private void HideLootPrefab() =>
            _lootPrefab.SetActive(false);

        private IEnumerator StartDestroyTimer()
        {
            yield return new WaitForSeconds(_destroyCooldown);

            Destroy(gameObject);
        }

        private void PlayPickupFx() =>
            Instantiate(_pickupFxPrefab, transform.position, Quaternion.identity);

        private void ShowText()
        {
            _lootText.text = $"{_loot.Value}";
            _pickupPopup.SetActive(true);
        }
    }
}