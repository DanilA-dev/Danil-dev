using System;
using Cysharp.Threading.Tasks;

namespace D_Dev.AdsService
{
    public interface IAdsModule
    {
        public bool IsInitialized { get; }
        public UniTask Initialize();
        public void Dispose();
        public void ShowBannerAd(Action<bool> callback);
        public void ShowInterstitialAd(Action<bool> callback);
        public void ShowRewardedAd(Action<bool> callback);
    }
}
