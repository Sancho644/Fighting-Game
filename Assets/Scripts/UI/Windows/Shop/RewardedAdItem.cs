using Infrastructure.Services.Ads;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Shop
{
    public class RewardedAdItem : WindowBase
    {
        [SerializeField] private Button _showAdButton;
        [SerializeField] private GameObject[] _adActiveObjects;
        [SerializeField] private GameObject[] _adInactiveObjects;

        private IAdsService _adsService;
        private IPersistentProgressService _progressService;

        public void Construct(IAdsService adsService, IPersistentProgressService progressService)
        {
            _adsService = adsService;
            _progressService = progressService;
        }

        public void InitializeReward()
        {
            _showAdButton.onClick.AddListener(OnShowAdClicked);

            RefreshAvailableAd();
        }

        public void Subscribe() =>
            _adsService.RewardedVideoReady += RefreshAvailableAd;

        public void CleanupReward() =>
            _adsService.RewardedVideoReady -= RefreshAvailableAd;

        private void OnShowAdClicked() => 
            _adsService.ShowRewardedVideo(OnVideoFinished);

        private void OnVideoFinished() => 
            _progressService.Progress.WorldData.LootData.Add(_adsService.Reward);

        private void RefreshAvailableAd()
        {
            bool videoReady = _adsService.IsRewardedVideoReady;

            foreach (GameObject adActiveObject in _adActiveObjects)
                adActiveObject.SetActive(videoReady);

            foreach (GameObject adInactiveObject in _adInactiveObjects)
                adInactiveObject.SetActive(!videoReady);
        }
    }
}