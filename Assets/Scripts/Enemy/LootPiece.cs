using System.Collections;
using Data;
using Infrastructure.Services.PersistentProgress;
using Logic;
using TMPro;
using UnityEngine;
using Utils.ObjectPool;

namespace Enemy
{
    public class LootPiece : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private GameObject _lootPrefab;
        [SerializeField] private GameObject _pickupFxPrefab;
        [SerializeField] private GameObject _pickupPopup;
        [SerializeField] private UniqueId _uniqueId;
        [SerializeField] private TextMeshPro _lootText;
        [SerializeField] private float _destroyCooldown;

        private Loot _loot;
        private bool _pickedUp;
        private WorldData _worldData;
        private string _id;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
        }

        public void Initialize(Loot loot)
        {
            _loot = loot;
        }

        private void Start()
        {
            _id = _uniqueId.Id;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_pickedUp)
                return;

            LootPieceDataDictionary lootPiecesOnScene = progress.WorldData.LootData.LootPiecesOnScene;

            if (!lootPiecesOnScene.Dictionary.ContainsKey(_id))
                lootPiecesOnScene.Dictionary
                    .Add(_id, new LootPieceData(transform.position.AsVectorData(), _loot));
        }

        public void LoadProgress(PlayerProgress progress)
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_pickedUp)
                return;

            _pickedUp = true;

            Pickup();
        }

        private void Pickup()
        {
            UpdateWorldData();
            HideLootPrefab();
            PlayPickupFx();
            ShowText();

            StartCoroutine(StartDestroyTimer());
        }

        private void UpdateWorldData()
        {
            _worldData.LootData.Collect(_loot);
            RemoveLootPieceFromSavedPieces();
        }

        private void RemoveLootPieceFromSavedPieces()
        {
            LootPieceDataDictionary savedLootPieces = _worldData.LootData.LootPiecesOnScene;

            if (savedLootPieces.Dictionary.ContainsKey(_id))
                savedLootPieces.Dictionary.Remove(_id);
        }

        private void HideLootPrefab() =>
            _lootPrefab.SetActive(false);

        private IEnumerator StartDestroyTimer()
        {
            yield return new WaitForSeconds(_destroyCooldown);

            Destroy(gameObject);
        }

        private void PlayPickupFx()
        {
            Pool.Instance.Get(_pickupFxPrefab, transform.position);
        }

        private void ShowText()
        {
            _lootText.text = _loot.Value.ToString();
            _pickupPopup.SetActive(true);
        }
    }
}