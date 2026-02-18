using System;

namespace D_Dev.AdsService
{
    public interface IAdsModule
    {
        public bool IsInitialized { get; }
        public void Initialize();
        public void ShowBannerAd(Action<bool> callback);
        public void ShowInterstitialAd(Action<bool> callback);
        public void ShowRewardedAd(Action<bool> callback);
    }
}
