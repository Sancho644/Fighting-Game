using System.Threading.Tasks;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.PersistentProgress.SaveLoad;
using Logic;
using UI.Services.Factory;
using UI.Services.Windows;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string MainMenu = "MainMenu";
        
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uiFactory;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IWindowService _windowService;
        private IPersistentProgressService _progressService;

        public LoadLevelState(SceneLoader sceneLoader, LoadingCurtain curtain, IGameFactory gameFactory,
            IUIFactory uiFactory, ISaveLoadService saveLoadService, IWindowService windowService,
            IPersistentProgressService progressService)
        {
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
            _uiFactory = uiFactory;
            _saveLoadService = saveLoadService;
            _windowService = windowService;
            _progressService = progressService;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _gameFactory.CleanUp();
            _gameFactory.WarmUp();
            _uiFactory.Construct(_gameFactory, _saveLoadService, _curtain, _windowService);
            _sceneLoader.Load(sceneName, LoadMainMenu);
        }

        public void Exit()
        {
        }

        private void LoadMainMenu()
        {
            _sceneLoader.Load(MainMenu, OnLoaded);
        }

        private async void OnLoaded()
        {
            await _gameFactory.CreateAudioSources();
            await InitUIRoot();
            InitMainMenu();
            
            InformProgressReaders();
        }

        private async Task InitUIRoot() =>
            await _uiFactory.CreateUIRoot();

        private void InitMainMenu()
        {
            _uiFactory.CreateMainMenu();
            _curtain.Hide();
        }
        
        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }
    }
}