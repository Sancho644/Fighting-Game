using Infrastructure.AssetManagement;
using Infrastructure.Services.Ads;
using Infrastructure.Services.IAP;
using Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Shop
{
    public class ShopWindow : WindowBase
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private TextMeshProUGUI _moneyText;
        
        [field: SerializeField] public Transform AdItemContainer { get; private set; }
        [field: SerializeField] public Transform ShopItemsContainer { get; private set; }
        
        private RewardedAdItem _adItem;
        private ShopItems _shopItems;

        protected override void OnAwake() => 
            _closeButton.onClick.AddListener(() => Destroy(gameObject));

        public void Construct(RewardedAdItem rewardedAdItem, ShopItems  shopItems, IAdsService adsService, IPersistentProgressService progressService, IIAPService iapService, IAssets assets)
        {
            base.Construct(progressService);

            _adItem = rewardedAdItem;
            _shopItems = shopItems;

            _adItem.Construct(adsService, progressService);
            _shopItems.Construct(iapService, progressService, assets);
        }

        protected override void Initialize()
        {
            _adItem.InitializeReward();
            _shopItems.InitializeItems();
            
            RefreshMoneyText();
        }

        protected override void SubscribeUpdates()
        {
            _adItem.Subscribe();
            _shopItems.Subscribe();
            Progress.WorldData.LootData.Changed += RefreshMoneyText;
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            
            _adItem.CleanupReward();
            _shopItems.CleanupItems();
            
            Progress.WorldData.LootData.Changed -= RefreshMoneyText;
        }

        private void RefreshMoneyText() => 
            _moneyText.text = Progress.WorldData.LootData.Collected.ToString();
    }
}