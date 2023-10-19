using Data;
using Infrastructure;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.PersistentProgress.SaveLoad;
using Logic;
using UI.Services.Factory;
using UI.Services.Windows;
using UnityEngine;

namespace UI.Windows.MainMenu
{
    public class MainMenuWindowController : WindowBase
    {
        private const string LevelOne = "Level1";

        [field: SerializeField] public Transform ContentContainer { get; private set; }

        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;
        private SceneLoader _sceneLoader;
        private MainMenuWindow _mainMainMenu;
        private IInitGameWorldService _initGameWorldService;
        private LoadingCurtain _curtain;
        private IWindowService _windowService;
        private IUIFactory _uiFactory;

        public void Construct(MainMenuWindow mainMenuWindow, IPersistentProgressService progressService,
            ISaveLoadService saveLoadService, SceneLoader sceneLoader, IInitGameWorldService initGameWorldService,
            LoadingCurtain curtain, IWindowService windowService, IUIFactory uiFactory)
        {
            _mainMainMenu = mainMenuWindow;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _sceneLoader = sceneLoader;
            _initGameWorldService = initGameWorldService;
            _curtain = curtain;
            _windowService = windowService;
            _uiFactory = uiFactory;
        }

        protected override void Initialize()
        {
            InitButtons();
        }

        private void InitButtons()
        {
            _mainMainMenu.SetStartButtonCallBack(OnStart);
            _mainMainMenu.SetSettingsButtonCallBack(OnSettings);
            _mainMainMenu.SetExitButtonCallBack(OnExit);

            PlayerProgress progress = _saveLoadService.LoadProgress();
            if (progress != null)
                _mainMainMenu.SetContinueButtonCallBack(OnContinue);
        }

        private void OnStart()
        {
            _sceneLoader.Load(LevelOne, OnLoaded);
            _curtain.Show();
            _progressService.Progress = _initGameWorldService.NewProgress();
        }

        private async void OnContinue()
        {
            _sceneLoader.Load(_progressService.Progress.WorldData.PositionOnLevel.Level, OnLoaded);
            _progressService.Progress = _initGameWorldService.LoadProgress();
            await _uiFactory.CreateUIRoot();
            _curtain.Show();
        }

        private void OnSettings()
        {
            _windowService.Open(WindowId.SettingsWindowController);
        }

        private void OnExit()
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private async void OnLoaded()
        {
            await _initGameWorldService.InitGameWorld();
            _initGameWorldService.InformProgressReaders();
            await _uiFactory.CreateUIRoot();
            _curtain.Hide();
        }
    }
}