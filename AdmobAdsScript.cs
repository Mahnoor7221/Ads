using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
//using static System.Net.Mime.MediaTypeNames;


public class AdmobAdsScript : MonoBehaviour
{
    public Text totalCoinsTxt;
    public string appId = "";





#if UNITY_ANDROID
    string bannerId = "ca-app-pub-3940256099942544/6300978111";
    string interId = "ca-app-pub-3940256099942544/1033173712";
    string rewardedId = "ca-app-pub-3940256099942544/5224354917";
    string nativeId = "ca-app-pub-3940256099942544/2247696110";

#elif UNITY_IPHONE
    string bannerId = "ca-app-pub-3940256099942544/2934735716";
    string interId = "ca-app-pub-3940256099942544/4411468910";
    string rewardedId = "ca-app-pub-3940256099942544/1712485313";
    string nativeId = "ca-app-pub-3940256099942544/3986624511";

#endif
    BannerView bannerView;
    InterstitialAd interstitialAd;
    RewardedAd rewardedAd;
    NativeAd nativeAd;


    private void Start()
    {
        ShowCoins();
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => {

            print("Ads Initialised   !!");

        });
    }
    #region Banner

    public void LoadBannerAd()
    {
        // create a banner
        CreateBannerView();

        // listen to banner events
        ListenToBannerEvents();

        // load the banner
        if (bannerView == null)
        {
            CreateBannerView();
        }
        var adrequest = new AdRequest();
        adrequest.Keywords.Add("unity-admob-sample");

        print("loading banner Ad !!");
        bannerView.LoadAd(adrequest); // show the banner on the screen

    }

    void CreateBannerView()
    {
        if (bannerView != null)
        {
            DestroyBannerAd();
        }
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Top);
    }

    void ListenToBannerEvents()
    {
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            /* Debug.Log("Banner view paid {0} {1}.",
                 adValue.Value+ adValue.CurrencyCode);*/
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }
    public void DestroyBannerAd()
    {

        if (bannerView != null)
        {
            print("Destroying banner Ad");
            bannerView.Destroy();
            bannerView = null;

        }
    }
    #endregion


    #region Interstitial
    public void LoadInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
        var adrequest = new AdRequest();
        adrequest.Keywords.Add("unity-admob-sample");

        interstitialAd.Load(interId, AdRequest (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)

            {
                print("interstitial ad failed to load" + error);
                return;
            }

            print("Interstitial ad loaded !!" + ad.GetResponseInfo());

            interstitialAd = ad;
            InterstitialEvent(interstitialAd);
        });


    }
    public void ShowInterstitialAd()
    {
        if (interstitialAd! = null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            print("interstitial ad not ready !!");
        }


    }
    public void InterstitialEvent(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Interstitial ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
    #endregion


    #region Rewarded

    public void LoadRewardedAd() {
        if (rewardedAd! = null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }
        var adrequest = new AdRequest();
        adrequest.Keywords.Add("unity-admob-sample");

        rewardedAd.Load(rewardedId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error! = null || ad == null
            {
                print("Rewarded failed to load" + error);
                return;
            }
            print("Rewarded ad loaded !!");
            rewardedAd = ad;
            RewardedAdEvents(rewardedAd);
        });
    }
    public void ShowRewardedAd()
    {

        if (rewardedAd! = null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) => {
                print("Give reward to player !!");

                GrantCoins(100);
            });
        }
        else
        {
            print("rewarded ad not ready");
        }
    }
    public void RewardedAdEvents(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Rewarded ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };

    }
    #endregion


    #region Native
    public Image img;

    public void RequestNativeAd()
    {

        AdLoader adLoaded = new AdLoader.Builder(nativeId).ForNativeAd().Build();

        adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
        adLoader.OnAdFailedToLoad += this.HandleNativeAdFailedToLoad;

        adLoader.LOadad(new AdRequest.Builder().Build());
    }
    private void HandleNativeAdLoaded(object sender, NativeAdEventsArgs e);
   { 
    print (“Native ad loaded”);
        this.nativeAd = e.nativeAd;

Texture2D iconTexture = this.nativeAd.GetIconTexture();
    Sprite sprite = Sprite.Create(iconTexture, newRect(0, 0, iconTexture.width, iconTexture.heigh), Vector2.one * .5f);
    Img.sprite = sprite;
}
     










#endregion

#region extra


void GrantCoins(int coins)
    {
        int crrCoins = PlayerPrefs.GetInt("totalCoins");
        crrCoins += coins;
        PlayerPrefs.SetInt("totalCoins", crrCoins);


        ShowCoins();

    }
    void ShowCoins()
    {
        totalCoinsTxt.text = PlayerPrefs.GetInt("").ToString();
    }



}