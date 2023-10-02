using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Infrastructure.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsShowListener, IUnityAdsLoadListener, IUnityAdsInitializationListener
    {
        private const string RewardedVideoPlacementId = "Rewarded_Android";
        private const string AndroidGameId = "5431165";
        private const string IOSGameId = "5431164";

        private string _gameId;
        private Action _onVideoFinished;

        public event Action RewardedVideoReady;

        public int Reward => 13;

        public void Initialize()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _gameId = AndroidGameId;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _gameId = IOSGameId;
                    break;
                case RuntimePlatform.WindowsEditor:
                    _gameId = AndroidGameId;
                    break;
                default:
                    Debug.Log("Unsupported platform for ads");
                    break;
            }

            Advertisement.Initialize(_gameId, true, this);
        }

        private void LoadRewardedAd()
        {
            Advertisement.Load(RewardedVideoPlacementId, this);
        }

        public void ShowRewardedVideo(Action onVideoFinished)
        {
            LoadRewardedAd();

            _onVideoFinished = onVideoFinished;
        }

        public bool IsRewardedVideoReady =>
            Advertisement.isInitialized;

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) =>
            Debug.Log($"OnUnityAdsShowFailure{message}, {error}");

        public void OnUnityAdsShowStart(string placementId) =>
            Debug.Log($"OnUnityAdsShowStart{placementId}");

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"OnUnityAdsShowClick{placementId}");
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            switch (showCompletionState)
            {
                case UnityAdsShowCompletionState.UNKNOWN:
                    Debug.LogError($"OnUnityAdsShowComplete{showCompletionState}");
                    break;
                case UnityAdsShowCompletionState.SKIPPED:
                    Debug.LogError($"OnUnityAdsShowComplete{showCompletionState}");
                    break;
                case UnityAdsShowCompletionState.COMPLETED:
                    _onVideoFinished?.Invoke();
                    break;
                default:
                    Debug.LogError($"OnUnityAdsShowComplete{showCompletionState}");
                    break;
            }

            _onVideoFinished = null;
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message) =>
            Debug.Log($"OnUnityAdsShowFailure{message}, {error}");

        public void OnInitializationComplete()
        {
            Debug.Log("OnInitializationComplete");

            RewardedVideoReady?.Invoke();
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Advertisement.Show(RewardedVideoPlacementId, this);

            Debug.Log($"OnUnityAdsAdLoaded{placementId}");
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) =>
            Debug.Log($"OnUnityAdsFailedToLoad{placementId}, {error}, {message}");
    }
}