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


        public delegate void Action(LootPiece lootPiece);

        public event Action OnSpawn;
        
        public void Construct(IGameFactory factory, IRandomService random)
        {
            _factory = factory;
            _random = random;
        }

        private void Start()
        {
            _enemyDeath.Happened += SpawnLoot;
        }

        private void SpawnLoot()
        {
            var lootPiece = _factory.CreateLoot(transform.position);
            //lootPiece.transform.position = transform.position;
            
            var lootItem = GenerateLoot();
            lootPiece.Initialize(lootItem);

            OnSpawn?.Invoke(lootPiece);
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