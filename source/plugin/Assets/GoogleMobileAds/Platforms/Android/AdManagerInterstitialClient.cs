// Copyright (C) 2015 Google, Inc.
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

using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Android
{
    public class AdManagerInterstitialClient : InterstitialClient
    {
        private AndroidJavaObject androidAdmanagerInterstitialAd;

        public AdManagerInterstitialClient() : base(Utils.UnityInterstitialAdCallbackClassName)
        {
            AndroidJavaClass playerClass = new AndroidJavaClass(Utils.UnityActivityClassName);
            AndroidJavaObject activity =
                    playerClass.GetStatic<AndroidJavaObject>("currentActivity");
            this.androidAdmanagerInterstitialAd = new AndroidJavaObject(
                Utils.AdManagerInterstitialClassName, activity, this);
        }

        public event EventHandler<AppEventEventArgs> OnAppEvent;

        #region IInterstitialClient implementation

        // Loads an AdManager Interstitial ad.
        public void LoadAd(string adUnitId, AdRequest request)
        {
            this.androidAdmanagerInterstitialAd.Call("loadAd", adUnitId,
                    Utils.GetAdManagerAdRequestJavaObject(request));
        }

        // Presents the interstitial ad on the screen.
        public void Show()
        {
            this.androidAdmanagerInterstitialAd.Call("show");
        }

        // Destroys the interstitial ad.
        public void DestroyInterstitial()
        {
            this.androidAdmanagerInterstitialAd.Call("destroy");
        }

        // Returns ad request response info
        public IResponseInfoClient GetResponseInfoClient()
        {
            return new ResponseInfoClient(ResponseInfoClientType.AdLoaded,
                                          this.androidAdmanagerInterstitialAd);
        }

        #endregion

        public void onAppEvent(string name, string data)
        {
            if (this.OnAppEvent != null)
            {
                AppEvent appEvent = new AppEvent()
                {
                    Name = name,
                    Data = data,
                };

                AppEventEventArgs args = new AppEventEventArgs()
                {
                    AppEvent = appEvent
                };

                this.OnAppEvent(this, args);
            }
        }
    }
}