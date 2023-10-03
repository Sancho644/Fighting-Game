using Data;
using Infrastructure.Factory;
using Infrastructure.Services.Randomizer;
using UnityEngine;

namespace Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyDeath _enemyDeath;

        private IGameFactory _factory;
        private IRandomService _random;

        private int _lootMin;
        private int _lootMax;

        public void Construct(IGameFactory factory, IRandomService random)
        {
            _factory = factory;
            _random = random;
        }

        private void Start()
        {
            _enemyDeath.Happened += SpawnLoot;
        }

        private async void SpawnLoot()
        {
            LootPiece lootPiece = await _factory.CreateLoot();
            lootPiece.transform.position = transform.position;

            Loot lootItem = GenerateLoot();
            lootPiece.Initialize(lootItem);
        }

        private Loot GenerateLoot()
        {
            return new Loot()
            {
                Value = _random.Next(_lootMin, _lootMax)
            };
        }

        public void SetLoot(int min, int max)
        {
            _lootMin = min;
            _lootMax = max;
        }
    }
}