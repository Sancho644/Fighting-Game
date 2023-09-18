using Data;

namespace Infrastructure.Services.PersistentProgress.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        void SaveProgress();
        PlayerProgress LoadProgress();
    }
}