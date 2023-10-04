using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services;
using StaticData.Windows;
using UI.Services.Windows;
using UnityEngine;

namespace StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string StaticDataEnemiesPath = "StaticData/Enemies";
        private const string StaticDataLevelsPath = "StaticData/Levels";
        private const string StaticDataWindowsPath = "StaticData/UI/WindowsStaticData";

        private Dictionary<EnemyTypeId, EnemyStaticData> _enemyes;
        private Dictionary<WindowId, WindowConfig> _windowConfigs;
        private Dictionary<string, LevelStaticData> _levels;

        public void LoadEnemies()
        {
            _enemyes = Resources
                .LoadAll<EnemyStaticData>(StaticDataEnemiesPath)
                .ToDictionary(x => x.EnemyTypeId, x => x);

            _levels = Resources
                .LoadAll<LevelStaticData>(StaticDataLevelsPath)
                .ToDictionary(x => x.LevelKey, x => x);

            _windowConfigs = Resources
                .Load<WindowsStaticData>(StaticDataWindowsPath)
                .Configs
                .ToDictionary(x => x.WindowId, x => x);
        }

        public EnemyStaticData ForEnemy(EnemyTypeId enemyId) =>
            _enemyes.TryGetValue(enemyId, out EnemyStaticData staticData) ? staticData : null;

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels.TryGetValue(sceneKey, out LevelStaticData staticData) ? staticData : null;

        public WindowConfig ForWindow(WindowId windowId) =>
            _windowConfigs.TryGetValue(windowId, out WindowConfig windowConfig) ? windowConfig : null;
    }
}