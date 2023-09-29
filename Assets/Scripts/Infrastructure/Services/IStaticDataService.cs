using StaticData;
using StaticData.Windows;
using UI.Services.Windows;

namespace Infrastructure.Services
{
    public interface IStaticDataService : IService
    {
        void LoadEnemies();
        EnemyStaticData ForEnemy(EnemyTypeId enemyId);
        LevelStaticData ForLevel(string sceneKey);
        WindowConfig ForWindow(WindowId shop);
    }
}