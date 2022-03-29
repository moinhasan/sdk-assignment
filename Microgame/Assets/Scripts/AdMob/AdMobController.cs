using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class AdMobController : Singleton<AdMobController>
{
    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private float deltaTime;

    #region UNITY MONOBEHAVIOR METHODS

    public void Start()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);

        // Add some test device IDs (replace with your own device IDs).
        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };
#if UNITY_IPHONE
        deviceIds.Add("96e23e80653bb28980d3f40beb58915c");
#elif UNITY_ANDROID
        deviceIds.Add(SystemInfo.deviceUniqueIdentifier);
#endif
        // Configure TagForChildDirectedTreatment and test device IDs.
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
            .SetTestDeviceIds(deviceIds).build();
        MobileAds.SetRequestConfiguration(requestConfiguration);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);
    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            Debug.Log("AdMob Initialization complete");
            RequestBannerAd();
            RequestInterstitialAd();
        });
    }

    #endregion

    #region HELPER METHODS

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
            .AddKeyword("unity-admob-sample")
            .Build();
    }

    #endregion

    #region BANNER ADS

    public void RequestBannerAd()
    {
        Debug.Log("Requesting Banner Ad.");

        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    #endregion

    #region INTERSTITIAL ADS

    public void RequestInterstitialAd()
    {
        Debug.Log("Requesting Interstitial Ad.");

#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
        interstitialAd = new InterstitialAd(adUnitId);

        // Add Event Handlers
        AddInterstitialEventHandlers();

        // Load an interstitial ad
        interstitialAd.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            RequestInterstitialAd();
            Debug.Log("Interstitial ad is not ready yet");
        }
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }

    void AddInterstitialEventHandlers() {
        if (interstitialAd != null)
        {
            // Called when an ad request has successfully loaded.
            interstitialAd.OnAdLoaded += HandleAdLoaded;
            // Called when an ad request failed to load.
            interstitialAd.OnAdFailedToLoad += HandleAdFailedToLoad;
            // Called when an ad is shown.
            interstitialAd.OnAdOpening += HandleAdOpening;
            // Called when the ad is closed.
            interstitialAd.OnAdClosed += HandleAdClosed;
        }
    }

    void RemoveInterstitialEventHandlers()
    {
        if (interstitialAd != null)
        {
            interstitialAd.OnAdLoaded -= HandleAdLoaded;
            interstitialAd.OnAdFailedToLoad -= HandleAdFailedToLoad;
            interstitialAd.OnAdOpening -= HandleAdOpening;
            interstitialAd.OnAdClosed -= HandleAdClosed;
        }
    }

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.LoadAdError.GetMessage());
    }

    public void HandleAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpening event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    #endregion

    #endregion

    void OnEnable()
    {
        // Add Event Handlers
        AddInterstitialEventHandlers();
    }

    void OnDisable()
    {
        // Remove Event Handlers
        RemoveInterstitialEventHandlers();
    }


}
