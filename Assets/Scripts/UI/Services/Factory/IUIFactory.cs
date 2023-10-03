using System.Threading.Tasks;
using Infrastructure.Services;

namespace UI.Services.Factory
{
    public interface IUIFactory : IService
    {
        void CreateShop();
        Task CreateUIRoot();
    }
}