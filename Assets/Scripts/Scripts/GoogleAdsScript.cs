using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class GoogleAdsScript : MonoBehaviour
{
  public RewardBasedVideoAd rewardBasedVideo;

  //public Text testText;

  // Use this for initialization
  void Start()
  {
#if UNITY_ANDROID
    string appId = "ca-app-pub-3940256099942544~3347511713";
#elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
#else
            string appId = "unexpected_platform";
#endif
    // Initialize the Google Mobile Ads SDK.
    if ( !GameSystem.isGoogleAdsInit )
    {
      MobileAds.Initialize(appId);
      GameSystem.isGoogleAdsInit = true;
    }
      

    this.rewardBasedVideo = RewardBasedVideoAd.Instance;

     // Called when an ad request has successfully loaded.
     rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
     // Called when an ad request failed to load.
     rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
     // Called when an ad is shown.
     rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
     // Called when the ad starts to play.
     rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
     // Called when the user should be rewarded for watching a video.
     rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
     // Called when the ad is closed.
     rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
     // Called when the ad click caused the user to leave the application.
     rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
    //Debug.Log("TO LOGCASADDDDDDDDDDDDDDDDDDDD");
    this.RequestRewardVideo();
  }

  private void OnDestroy()
  {
    //rewardBasedVideo.OnAdClosed -= RewardVideoClosed;
  }
  // Update is called once per frame
  void Update()
  {

  }

  public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
  {
    
    Debug.Log("Video loaded");
    //MonoBehaviour.print("asdasdasdas");
    //testText.text = "HandleRewardBasedVideoLoaded event received";
  }

  public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
  {

   // testText.text = "HandleRewardBasedVideoFailedToLoad event received with message: ";
  }

  public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
  {
  //  testText.text =  "HandleRewardBasedVideoOpened event received";
  }

  public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
  {
  //  testText.text = "HandleRewardBasedVideoStarted event received";
  }

  public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
  {
   // Debug.Log("Video Closed");
    //GameSystem.isAdClosed = true;
    this.RequestRewardVideo();
    //EventsManager.TriggerEvent(EventsIds.CLOSE_REWARD_VIDEO);
    //Debug.Log("Video Closed");

    //  testText.text = "HandleRewardBasedVideoClosed event received";
  }

  public void HandleRewardBasedVideoRewarded(object sender, Reward args)
  {
    //Debug.Log("Video Rewarded");
    GameSystem.isAdClosed = true;
    //EventsManager.TriggerEvent(EventsIds.CLOSE_REWARD_VIDEO);
    /*  string type = args.Type;
      double amount = args.Amount;*/
    /* testText.text =
        "HandleRewardBasedVideoRewarded event received for "
                     + amount.ToString() + " " + type;*/
  }

  public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
  {
   // testText.text = "HandleRewardBasedVideoLeftApplication event received";
  }

  public void ShowRewardVideo()
  {
    //testText.text = "RewardVideoShow;";
    //Debug.Log("ShowVideoPressed");
    rewardBasedVideo.Show();
  }


  public void RewardVideoClosed(object sender, EventArgs args)
  {
    //testText.text = "RewardVideoClosed;";
    //GameUIController.instance.ReviveForVideo();
    this.RequestRewardVideo();
  }

  private void RequestRewardVideo()
  {
#if UNITY_ANDROID
    string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            string adUnitId = "unexpected_platform";
#endif

    // Create an empty ad request.
    AdRequest request = new AdRequest.Builder().Build();
    // Load the rewarded video ad with the request.
    this.rewardBasedVideo.LoadAd(request, adUnitId);
  }


  private void RequestBanner()
  {
#if UNITY_ANDROID
    string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
          string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
          string adUnitId = "unexpected_platform";
#endif

    // Create a 320x50 banner at the top of the screen.
    BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
    // Create an empty ad request.
    AdRequest request = new AdRequest.Builder().Build();
    // Load the banner with the request.
    bannerView.LoadAd(request);
  }

  public void GetBanner()
  {
    RequestBanner();
  }
}
