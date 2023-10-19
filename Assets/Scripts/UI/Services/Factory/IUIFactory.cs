using System.Threading.Tasks;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress.SaveLoad;
using Logic;
using UI.Services.Windows;

namespace UI.Services.Factory
{
    public interface IUIFactory : IService
    {
        void CreateShop();
        Task CreateUIRoot();
        void CreateMainMenu();
        void Construct(IGameFactory gameFactory, ISaveLoadService saveLoadService, LoadingCurtain curtain,
            IWindowService windowService);

        void CreateSettings();
    }
}