using System.Threading.Tasks;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.AssetManagement
{
    public interface IAssets : IService
    {
        Task<GameObject> Instantiate(string address);
        Task<GameObject> Instantiate(string address, Vector3 at);
        Task<GameObject> Instantiate(string address, Transform under);
        Task<T> Load<T>(AssetReference assetReference) where T : class;
        Task<T> Load<T>(string address) where T : class;
        void CleanUp();
        void Instantiate();
    }
}