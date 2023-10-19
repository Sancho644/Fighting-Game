using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.AssetManagement;
using Infrastructure.Services.IAP;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace UI.Windows.Shop
{
    public class ShopItems : WindowBase
    {
        private const string ShopItemPath = "ShopItem";

        [SerializeField] private GameObject[] _shopUnavailableObjects;
        [SerializeField] private Transform _parent;

        private IIAPService _iapService;
        private IPersistentProgressService _progressService;
        private IAssets _assets;

        private readonly List<GameObject> _shopItems = new List<GameObject>();

        public void Construct(IIAPService iapService, IPersistentProgressService progressService, IAssets assets)
        {
            _iapService = iapService;
            _progressService = progressService;
            _assets = assets;
        }

        public void InitializeItems() =>
            RefreshAvailableItems();

        public void Subscribe()
        {
            _iapService.Initialized += RefreshAvailableItems;
            _progressService.Progress.PurchaseData.Changed += RefreshAvailableItems;
        }

        public void CleanupItems()
        {
            _iapService.Initialized -= RefreshAvailableItems;
            _progressService.Progress.PurchaseData.Changed -= RefreshAvailableItems;
        }

        private async void RefreshAvailableItems()
        {
            UpdateShopUnavailableObjects();

            if (!_iapService.IsInitialized)
                return;

            ClearShopItems();

            await FillShopItems();
        }

        private void ClearShopItems()
        {
            foreach (GameObject shopItem in _shopItems)
                Destroy(shopItem);
        }

        private async Task FillShopItems()
        {
            foreach (ProductDescription productDescription in _iapService.Products())
            {
                GameObject shopItemObject = await _assets.Instantiate(ShopItemPath, _parent);
                ShopItem shopItem = shopItemObject.GetComponent<ShopItem>();

                shopItem.Construct(_iapService, _assets, productDescription);
                shopItem.Initialize();

                _shopItems.Add(shopItemObject);
            }
        }

        private void UpdateShopUnavailableObjects()
        {
            foreach (GameObject shopUnavailableObject in _shopUnavailableObjects)
                shopUnavailableObject.SetActive(!_iapService.IsInitialized);
        }
    }
}