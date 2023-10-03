using Data;
using Enemy;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using StaticData;
using UnityEngine;

namespace Logic.EnemySpawners
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        public EnemyTypeId EnemyTypeId;

        private bool _slain;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;
        private GameObject _enemy;

        public string Id { get; set; }

        public void Construct(IGameFactory factory) => 
            _factory = factory;

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(Id))
                _slain = true;
            else
                Spawn();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_slain)
                progress.KillData.ClearedSpawners.Add(Id);
        }

        private async void Spawn()
        {
            _enemy = await _factory.CreateEnemy(EnemyTypeId, transform);
            _enemyDeath = _enemy.GetComponent<EnemyDeath>();

            _enemyDeath.Happened += Slay;
        }

        private void Slay()
        {
            if (_enemyDeath != null)
                _enemyDeath.Happened -= Slay;

            _slain = true;
        }
    }
}