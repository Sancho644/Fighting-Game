using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services;
using UnityEngine;

namespace StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<EnemyTypeId,EnemyStaticData> _enemyes;

        public void LoadEnemies()
        {
            _enemyes = Resources
                .LoadAll<EnemyStaticData>("StaticData/Enemies")
                .ToDictionary(x => x.EnemyTypeId, x => x);
        }

        public EnemyStaticData ForEnemy(EnemyTypeId enemyId) => 
            _enemyes.TryGetValue(enemyId, out EnemyStaticData staticData) ? staticData : null;
    }
}