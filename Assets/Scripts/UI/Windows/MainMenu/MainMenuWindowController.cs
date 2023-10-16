using Infrastructure;
using Infrastructure.Services;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.PersistentProgress.SaveLoad;
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

        public void Construct(MainMenuWindow mainMenuWindow, IPersistentProgressService progressService, ISaveLoadService saveLoadService, SceneLoader sceneLoader, IInitGameWorldService initGameWorldService)
        {
            _mainMainMenu = mainMenuWindow;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
            _sceneLoader = sceneLoader;
            _initGameWorldService = initGameWorldService;
        }

        private void Start()
        {
            InitButtons();
        }

        private void InitButtons()
        {
            _mainMainMenu.SetStartButtonCallBack(OnStart);
            _mainMainMenu.SetSettingsButtonCallBack(OnSettings);
            _mainMainMenu.SetExitButtonCallBack(OnExit);

            var progress = _saveLoadService.LoadProgress();
            if (progress != null)
                _mainMainMenu.SetContinueButtonCallBack(OnContinue);
        }

        private void OnStart()
        {
            _sceneLoader.Load(LevelOne, OnLoaded);
            _progressService.Progress = _initGameWorldService.NewProgress();
        }

        private void OnContinue()
        {
            _sceneLoader.Load(_progressService.Progress.WorldData.PositionOnLevel.Level, OnLoaded);
            _progressService.Progress = _initGameWorldService.LoadProgress();
        }

        private void OnSettings()
        {
            Debug.Log("OnSetting");
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
        }
    }
}