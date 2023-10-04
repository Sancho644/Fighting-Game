using System;
using System.Collections.Generic;

namespace Infrastructure.Services.IAP
{
    public interface IIAPService : IService
    {
        bool IsInitialized { get; }
        event Action Initialized;
        void Initialize();
        void StartPurchase(string productId);
        List<ProductDescription> Products();
    }
}