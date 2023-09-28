using StaticData;

namespace Infrastructure.Services
{
    public interface IStaticDataService : IService
    {
        void LoadEnemies();
        EnemyStaticData ForEnemy(EnemyTypeId enemyId);
        LevelStaticData ForLevel(string sceneKey);
    }
}