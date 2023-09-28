﻿using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services;
using UnityEngine;

namespace StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemyes;
        private Dictionary<string, LevelStaticData> _levels;

        public void LoadEnemies()
        {
            _enemyes = Resources
                .LoadAll<EnemyStaticData>("StaticData/Enemies")
                .ToDictionary(x => x.EnemyTypeId, x => x);
            
            _levels = Resources
                .LoadAll<LevelStaticData>("StaticData/Levels")
                .ToDictionary(x => x.LevelKey, x => x);
        }

        public EnemyStaticData ForEnemy(EnemyTypeId enemyId) =>
            _enemyes.TryGetValue(enemyId, out EnemyStaticData staticData) ? staticData : null;

        public LevelStaticData ForLevel(string sceneKey) => 
            _levels.TryGetValue(sceneKey, out LevelStaticData staticData) ? staticData : null;
    }
}