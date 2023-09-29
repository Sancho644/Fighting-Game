using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Infrastructure.Services.Ads
{
    public class AdsService : IAdsService, IUnityAdsShowListener
    {
        private const string AndroidGameId = "5431165";
        private const string IOSGameId = "5431164";

        private const string RewardedVideoPlacementId = "Rewarded_Android";

        private string _gameId;
        private Action _onVideoFinished;

        public event Action RewardedVideoReady;

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
                    _gameId = IOSGameId;
                    break;
                default:
                    Debug.Log("Unsupported platform for ads");
                    break;
            }

            Advertisement.Initialize(_gameId);
        }

        public void ShowRewardedVideo(Action onVideoFinished)
        {
            Advertisement.Show(RewardedVideoPlacementId);

            _onVideoFinished = onVideoFinished;
        }

        public bool IsRewardedVideoReady => 
            Advertisement.isInitialized;

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) =>
            Debug.Log($"OnUnityAdsShowFailure{message}");

        public void OnUnityAdsShowStart(string placementId) =>
            Debug.Log($"OnUnityAdsShowStart{placementId}");

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"OnUnityAdsShowClick{placementId}");

            if (placementId == RewardedVideoPlacementId)
                RewardedVideoReady?.Invoke();
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
    }
}