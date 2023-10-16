using System.Threading.Tasks;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress.SaveLoad;
using Logic;
using UI.Services.Factory;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uiFactory;
        private readonly ISaveLoadService _saveLoadService;

        public LoadLevelState(SceneLoader sceneLoader, LoadingCurtain curtain, IGameFactory gameFactory,
            IUIFactory uiFactory, ISaveLoadService saveLoadService)
        {
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
            _saveLoadService = saveLoadService;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _gameFactory.CleanUp();
            _gameFactory.WarmUp();
            _uiFactory.Construct(_gameFactory, _saveLoadService);
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
        }

        private async void OnLoaded()
        {
            await InitUIRoot();
            InitMainMenu();
        }

        private async Task InitUIRoot() =>
            await _uiFactory.CreateUIRoot();

        private void InitMainMenu()
        {
            _uiFactory.CreateMainMenu();
            _curtain.Hide();
        }
    }
}