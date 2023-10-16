using System.Threading.Tasks;
using Data;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress.SaveLoad;

namespace Infrastructure.Services
{
    public interface IInitGameWorldService : IService
    {
        Task InitGameWorld();
        PlayerProgress LoadProgress();
        PlayerProgress NewProgress();
        void InformProgressReaders();
        void Construct(IGameFactory gameFactory, ISaveLoadService saveLoadService);
    }
}