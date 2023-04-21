using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Samples;

namespace GoogleMobileAds.Sample
{
    /// <summary>
    /// Demonstrates how to use the Google Mobile Ads rewarded interstitial ad.
    /// </summary>
    /// <remarks>
    /// Before loading ads, initialize the Mobile Ads SDK by calling `MobileAds.Initialize()`;.
    /// Mobile ads initialize needs to be called only once, ideally at app launch.
    /// See MobileAdsController.cs for an example of mobile ads initialization.
    /// <remarks>
    [AddComponentMenu("GoogleMobileAds/Samples/RewardedInterstitialAdController")]
    public class RewardedInterstitialAdController : MonoBehaviour
    {
        /// <summary>
        /// UI element activated when an ad is ready to show.
        /// </summary>
        public GameObject LoadedUI;

        // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
        private const string _adUnitId = "ca-app-pub-3940256099942544/5354046379";
#elif UNITY_IPHONE
        private const string _adUnitId = "ca-app-pub-3940256099942544/6978759866";
#else
        private const string _adUnitId = "unused";
#endif

        private RewardedInterstitialAd _rewardedInterstitialAd;

        /// <summary>
        /// Loads the ad.
        /// </summary>
        public void LoadAd()
        {
            // Clean up the old ad before loading a new one.
            if (_rewardedInterstitialAd != null)
            {
                DestroyAd();
            }

            Debug.Log("Loading rewarded interstitial ad.");

            // Create our request used to load the ad.
            var adRequest = new AdRequest.Builder()
                .AddKeyword("unity-admob-sample")
                .Build();

            // Send the request to load the ad.
            RewardedInterstitialAd.Load(_adUnitId, adRequest,
                (RewardedInterstitialAd ad, LoadAdError error) =>
                {
                    // If the operation failed for unknowen reason.
                    if (error == null && ad == null)
                    {
                        Debug.LogError("Rewarded interstitial ad failed to load an ad");
                        return;
                    }
                    // If the operation failed with a reason.
                    if (error != null)
                    {
                        Debug.LogError("Rewarded interstitial ad failed to load an ad with error : "
                                        + error);
                        return;
                    }

                    // The operation compleated successfully.
                    Debug.Log("Rewarded interstitial ad loaded with response : "
                        + ad.GetResponseInfo());
                    _rewardedInterstitialAd = ad;

                    // Register to ad events to extend functionality.
                    RegisterEventHandlers(ad);

                    // Inform the UI that the ad is ready.
                    LoadedUI?.SetActive(true);
                });
        }

        /// <summary>
        /// Shows the ad.
        /// </summary>
        public void ShowAd()
        {
            if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd())
            {
                _rewardedInterstitialAd.Show((Reward reward) =>
                {
                    Debug.Log("Rewarded interstitial ad rewarded : " + reward.Amount);
                });
            }
            else
            {
                Debug.LogError("Rewarded interstitial ad is not ready yet.");
            }

            // Inform the UI that the ad is not ready.
            LoadedUI?.SetActive(false);
        }

        /// <summary>
        /// Destroys the ad.
        /// </summary>
        public void DestroyAd()
        {
            if (_rewardedInterstitialAd != null)
            {
                Debug.Log("Destroying rewarded interstitial ad.");
                _rewardedInterstitialAd.Destroy();
                _rewardedInterstitialAd = null;
            }

            // Inform the UI that the ad is not ready.
            LoadedUI?.SetActive(false);
        }

        /// <summary>
        /// Logs the ResponseInfo.
        /// </summary>
        public void LogResponseInfo()
        {
            if (_rewardedInterstitialAd != null)
            {
                var responseInfo = _rewardedInterstitialAd.GetResponseInfo();
                if (responseInfo != null)
                {
                    UnityEngine.Debug.Log(responseInfo);
                }
            }
        }

        protected void RegisterEventHandlers(RewardedInterstitialAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(String.Format("Rewarded interstitial ad paid {0} {1}.",
                    adValue.Value,
                    adValue.CurrencyCode));
            };
            // Raised when an impression is recorded for an ad.
            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Rewarded interstitial ad recorded an impression.");
            };
            // Raised when a click is recorded for an ad.
            ad.OnAdClicked += () =>
            {
                Debug.Log("Rewarded interstitial ad was clicked.");
            };
            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Rewarded interstitial ad full screen content opened.");
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded interstitial ad full screen content closed.");
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded interstitial ad failed to open full screen content" +
                               " with error : " + error);
            };
        }
    }
}
