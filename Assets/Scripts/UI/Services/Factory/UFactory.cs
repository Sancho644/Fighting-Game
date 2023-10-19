using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Infrastructure.Services;
using Infrastructure.Services.Ads;
using Infrastructure.Services.IAP;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.PersistentProgress.SaveLoad;
using Logic;
using StaticData.Windows;
using UI.Services.Windows;
using UI.Windows.MainMenu;
using UI.Windows.Settings;
using UI.Windows.Shop;
using UnityEngine;

namespace UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UIRoot";

        private readonly IAssets _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private readonly IAdsService _adsService;
        private readonly IIAPService _iapService;
        private readonly SceneLoader _sceneLoader;
        private readonly IInitGameWorldService _initGameWorldService;
       
        private LoadingCurtain _curtain;
        private IWindowService _windowService;
        private ISaveLoadService _saveLoadService;
        private IGameFactory _gameFactory;

        private Transform _uiRoot;

        public UIFactory(IAssets assets, IStaticDataService staticData, IPersistentProgressService progressService,
            IAdsService adsService, IIAPService iapService, SceneLoader sceneLoader,
            IInitGameWorldService initGameWorldService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
            _adsService = adsService;
            _iapService = iapService;
            _sceneLoader = sceneLoader;
            _initGameWorldService = initGameWorldService;
        }

        public void Construct(IGameFactory gameFactory, ISaveLoadService saveLoadService, LoadingCurtain curtain,
            IWindowService windowService)
        {
            _saveLoadService = saveLoadService;
            _curtain = curtain;
            _windowService = windowService;
            _gameFactory = gameFactory;
            
            _initGameWorldService.Construct(gameFactory, _saveLoadService);
        }

        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.ShopWindow);
            ShopWindow window = Object.Instantiate(config.Prefab, _uiRoot) as ShopWindow;
            config = _staticData.ForWindow(WindowId.AddItem);
            RewardedAdItem rewardedAdItem = Object.Instantiate(config.Prefab, window.AdItemContainer) as RewardedAdItem;
            config = _staticData.ForWindow(WindowId.ShopItems);
            ShopItems shopItems = Object.Instantiate(config.Prefab, window.ShopItemsContainer) as ShopItems;
            window.Construct(rewardedAdItem, shopItems, _adsService, _progressService, _iapService, _assets);
        }

        public void CreateMainMenu()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.MainMenuController);
            MainMenuWindowController windowController = Object.Instantiate(config.Prefab, _uiRoot) as MainMenuWindowController;
            config = _staticData.ForWindow(WindowId.MainMenuWindow);
            MainMenuWindow mainMenuWindow = Object.Instantiate(config.Prefab, windowController.ContentContainer) as MainMenuWindow;
            windowController.Construct(mainMenuWindow, _progressService, _saveLoadService, _sceneLoader, _initGameWorldService, _curtain, _windowService, this);
        }

        public async Task CreateUIRoot()
        {
            if (_uiRoot != null)
                return;
            
            GameObject root = await _assets.Instantiate(UIRootPath);
            _uiRoot = root.transform;
        }

        public void CreateSettings()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.SettingsWindowController);
            SettingsWindowController windowController = Object.Instantiate(config.Prefab, _uiRoot) as SettingsWindowController;
            WindowConfig sfxConfig = _staticData.ForWindow(WindowId.SfxAudioWidget);
            AudioSettingWidget sfxObject = Object.Instantiate(sfxConfig.Prefab, windowController.SfxContainer) as AudioSettingWidget;
            WindowConfig musicConfig = _staticData.ForWindow(WindowId.MusicAudioWidget);
            AudioSettingWidget musicObject = Object.Instantiate(musicConfig.Prefab, windowController.MusicContainer) as AudioSettingWidget;
            WindowConfig settingsConfig = _staticData.ForWindow(WindowId.SettingsWindow);
            SettingsWindow settingsWindow = Object.Instantiate(settingsConfig.Prefab, windowController.SettingsWindowContainer) as SettingsWindow;
            
            windowController.SettingWidgets.Add(sfxObject);
            windowController.SettingWidgets.Add(musicObject);
            windowController.Construct(_gameFactory.SfxSource, _gameFactory.MusicSource, settingsWindow, _saveLoadService);
        }
    }
}