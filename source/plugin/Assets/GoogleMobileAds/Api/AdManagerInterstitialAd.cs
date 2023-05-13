
// Copyright (C) 2023 Google, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;

using GoogleMobileAds;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.Api.AdManager
{
    public class AdManagerInterstitialAd : InterstitialAd
    {
        public event Action<AppEvent> OnAppEventReceived;

        private AdManagerInterstitialAd(IInterstitialClient client)
        {
            _client = client;
            _canShowAd = true;
            RegisterAdEvents();
        }

        public static void Load(string adUnitId, AdRequest request,
                                Action<AdManagerInterstitialAd, LoadAdError> adLoadCallback)
        {
            if (adLoadCallback == null)
            {
                UnityEngine.Debug.LogError("adLoadCallback is null. No ad was loaded.");
                return;
            }

            var client = MobileAds.GetClientFactory().BuildAdManagerInterstitialClient();
            client.CreateInterstitialAd();
            client.OnAdLoaded += (sender, args) =>
            {
                adLoadCallback(new AdManagerInterstitialAd(client), null);
            };
            client.OnAdFailedToLoad += (sender, error) =>
            {
                var loadAdError = new LoadAdError(error.LoadAdErrorClient);
                adLoadCallback(null, loadAdError);
            };
            client.LoadAd(adUnitId, request);
        }

        protected internal override void RegisterAdEvents()
        {
            base.RegisterAdEvents();

            _client.OnAppEvent += (sender, args) =>
            {
                MobileAds.RaiseAction(() =>
                {
                    if (OnAppEventReceived != null)
                    {
                        OnAppEventReceived(args.AppEvent);
                    }
                });
            };
        }
    }
}
