using System.Collections.Generic;
using Enemy;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using StaticData;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreateHero(Vector3 at);
        GameObject CreateHud();
        LootPiece CreateLoot(Vector3 parent);
        GameObject CreateEnemy(EnemyTypeId enemyTypeId, Transform parent);
        void CreateSpawners(Vector3 at, string spawnerId, EnemyTypeId enemyTypeId);
        
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }

        void CleanUp();
    }
}