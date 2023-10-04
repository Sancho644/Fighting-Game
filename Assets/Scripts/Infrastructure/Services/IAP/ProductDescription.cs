using UnityEngine.Purchasing;

namespace Infrastructure.Services.IAP
{
    public class ProductDescription
    {
        public string Id;
        public Product Product;
        public ProductConfig Config;
        public int AvailablePurchaseLeft;
    }
}