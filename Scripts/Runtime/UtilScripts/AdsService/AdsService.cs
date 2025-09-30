using System;
using System.Collections.Generic;
using UnityEngine;

namespace D_Dev.AdsService
{
    #region Enums

    public enum AdType
    {
        Banner,
        Interstitial,
        Rewarded
    }

    #endregion
    
    public class AdsService : MonoBehaviour
    {
        #region Fields

        [SerializeField] private bool _loadAllTypesOnStart = true;
        [SerializeReference] private List<IAdsModule> _adsModules = new();

        private Dictionary<AdType, bool> _adTypes = new Dictionary<AdType, bool>
        {
            { AdType.Banner, true },
            { AdType.Interstitial, true },
            { AdType.Rewarded, true }
        };

        #endregion

        #region Monobehaviour

        private void Start() => InitializeAdsModules();

        #endregion

        #region Public
        public void SetAdTypeLoadState(AdType adType, bool isLoaded) => _adTypes[adType] = isLoaded;
        public void LoadInterstitialAd() => SetAdTypeLoadState(AdType.Interstitial, true);
        public void LoadRewardedAd() => SetAdTypeLoadState(AdType.Rewarded, true);
        public void LoadBannerAd() => SetAdTypeLoadState(AdType.Banner, true);
        
        public void LoadAllAdTypes()
        {
            foreach (var (adType, isLoaded) in _adTypes)
                SetAdTypeLoadState(adType, true);
        }


        public void ShowBanner(Action<bool> callback)
        {
            if (!_adTypes[AdType.Banner])
            {
                Debug.Log("[AdsService] Banner ad is not loaded");
                callback?.Invoke(false);
                return;
            }
        
            foreach (var module in _adsModules)
            {
                if (module.IsInitialized)
                {
                    module.ShowBannerAd((adShown) => {
                        Debug.Log("[AdsService] " + module.GetType().Name + " banner ad shown: " + adShown);
                        callback?.Invoke(adShown);
                    });
                    return;
                }
            }
        
            Debug.Log("[AdsService] No banner ad modules are initialized");
            callback?.Invoke(false);
        }

        public void ShowInterstitial(Action<bool> callback)
        {
            if (!_adTypes[AdType.Interstitial])
            {
                Debug.Log("[AdsService] Interstitial ad is not loaded");
                callback?.Invoke(false);
                return;
            }

            foreach (var module in _adsModules)
            {
                if (module.IsInitialized)
                {
                    module.ShowInterstitialAd(callback);
                    Debug.Log("[AdsService] " + module.GetType().Name + " interstitial ad shown: true");
                    return;
                }
            }

            Debug.Log("[AdsService] No interstitial ad modules are initialized");
            callback?.Invoke(false);
        }

        public void ShowRewarded(Action<bool> callback)
        {
            if (!_adTypes[AdType.Rewarded])
            {
                Debug.Log("[AdsService] Rewarded ad is not loaded");
                callback?.Invoke(false);
                return;
            }

            foreach (var module in _adsModules)
            {
                if (module.IsInitialized)
                {
                    module.ShowRewardedAd(callback);
                    Debug.Log("[AdsService] " + module.GetType().Name + " rewarded ad shown: true");
                    return;
                }
            }

            Debug.Log("[AdsService] No rewarded ad modules are initialized");
            callback?.Invoke(false);
        }
        #endregion

        #region Private
        private void InitializeAdsModules()
        {
            foreach (var module in _adsModules)
            {
                module.Initialize();
                Debug.Log($"[AdsModule] Initialized module {module.GetType().Name}");
            }
            
            if(_loadAllTypesOnStart)
                LoadAllAdTypes();
        }
        #endregion
    }
}
