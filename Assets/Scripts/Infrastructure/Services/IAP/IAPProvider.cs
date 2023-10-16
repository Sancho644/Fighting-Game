using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Infrastructure.Services.IAP
{
    public class IAPProvider : IDetailedStoreListener
    {
        private const string IAPConfigsPath = "IAP/products";

        private IStoreController _controller;
        private IExtensionProvider _extensions;
        private IAPService _iapService;

        public Dictionary<string, ProductConfig> Configs { get; private set; }
        public Dictionary<string, Product> Products { get; private set; }

        public event Action Initialized;
        public bool IsInitialized => _controller != null && _extensions != null;

        public async void Initialize(IAPService iapService)
        {
            _iapService = iapService;

            Configs = new Dictionary<string, ProductConfig>();
            Products = new Dictionary<string, Product>();

            Load();

            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach (ProductConfig productConfig in Configs.Values)
                builder.AddProduct(productConfig.Id, productConfig.ProductType);

            var options = new InitializationOptions()
                .SetEnvironmentName("environment");

            await UnityServices.InitializeAsync(options);

            UnityPurchasing.Initialize(this, builder);
        }

        public void StartPurchase(string productId) =>
            _controller.InitiatePurchase(productId);

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensions = extensions;

            foreach (Product product in _controller.products.all)
            {
                Products.Add(product.definition.id, product);
            }

            Initialized?.Invoke();

            Debug.Log("OnInitialized success");
        }

        public void OnInitializeFailed(InitializationFailureReason error) =>
            Debug.Log($"OnInitialized failed {error}");

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Debug.Log($"ProcessPurchase success {purchaseEvent.purchasedProduct.definition.id}");

            return _iapService.ProcessPurchase(purchaseEvent.purchasedProduct);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) =>
            Debug.Log(
                $"Product {product.definition.id} purchase failed, PurchaseFailureReason {failureReason}, transaction id {product.transactionID}");

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
        }

        private void Load() =>
            Configs = Resources
                .Load<TextAsset>(IAPConfigsPath)
                .text
                .ToDeserialized<ProductConfigWrapper>()
                .Configs
                .ToDictionary(x => x.Id, x => x);
    }
}