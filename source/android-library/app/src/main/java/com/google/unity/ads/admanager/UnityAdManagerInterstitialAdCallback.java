package com.google.unity.ads.admanager;

import com.google.unity.ads.UnityFullScreenContentCallback;
import com.google.unity.ads.UnityInterstitialAdCallback;
import com.google.unity.ads.UnityPaidEventListener;

/**
 * An interface form of {@link AdManagerInterstitialAdLoadCallback} that can be implemented via
 * {@code AndroidJavaProxy} in Unity to receive ad events synchronously.
 */
public interface UnityAdManagerInterstitialAdCallback
    extends UnityInterstitialAdCallback, UnityPaidEventListener, UnityFullScreenContentCallback {

  void onAppEvent(String name, String data);
}
