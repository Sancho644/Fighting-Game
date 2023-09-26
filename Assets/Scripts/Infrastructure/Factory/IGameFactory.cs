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
        GameObject CreateHero(GameObject at);
        GameObject CreateHud();
        LootPiece CreateLoot();
        GameObject CreateEnemy(EnemyTypeId enemyTypeId, Transform parent);

        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }

        void CleanUp();
        void Register(ISavedProgressReader progressReader);
    }
}