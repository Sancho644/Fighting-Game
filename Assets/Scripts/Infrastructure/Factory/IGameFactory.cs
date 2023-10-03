using System.Collections.Generic;
using System.Threading.Tasks;
using Enemy;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using StaticData;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        Task<GameObject> CreateHero(Vector3 at);
        Task<GameObject> CreateHud();
        Task<LootPiece> CreateLoot();
        Task<GameObject> CreateEnemy(EnemyTypeId enemyTypeId, Transform parent);
        Task CreateSpawner(Vector3 at, string spawnerId, EnemyTypeId enemyTypeId);
        
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }

        void CleanUp();
        Task WarmUp();
    }
}