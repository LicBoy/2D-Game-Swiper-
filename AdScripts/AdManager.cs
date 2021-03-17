using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    public bool isTesting = false;

    private string APP_ID = "ca-app-pub-5140417694522204~2011401730";

    private string interstitial_ID;

    //Actual ad ID *FOR REAL USE*
    //string interstitial_ID = "ca-app-pub-5140417694522204/1915068932";
    //Ad ID for TESTING
    //string interstitial_ID = "ca-app-pub-3940256099942544/1033173712";

    private InterstitialAd interstitial;

    // Start is called before the first frame update
    void Start()
    {
        //when published game
        MobileAds.Initialize(APP_ID);

        if(!isTesting)
            interstitial_ID = "ca-app-pub-5140417694522204/1915068932";
        else
            interstitial_ID = "ca-app-pub-3940256099942544/1033173712";

        RequestInterstitial();
    }

    void RequestInterstitial()
    {
        if (interstitial != null)
            interstitial.Destroy();

        interstitial = new InterstitialAd(interstitial_ID);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;


        AdRequest adRequest;
        if(!isTesting)
        {
             adRequest = new AdRequest.Builder().Build();
        }
        else //FOR TESTING
        {
            adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
        }
        
        interstitial.LoadAd(adRequest);
    }

    public void DisplayInterstitial()
    {
        if(interstitial.IsLoaded())
            interstitial.Show();
    }

    // HANDLE EVENTS 

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
        UnsubscrubeEvents();
        RequestInterstitial();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        UnsubscrubeEvents();
        RequestInterstitial();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
        UnsubscrubeEvents();
    }

    void UnsubscrubeEvents()
    {
        // Called when an ad request has successfully loaded.
        interstitial.OnAdLoaded -= HandleOnAdLoaded;
        // Called when an ad request failed to load.
        interstitial.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        interstitial.OnAdOpening -= HandleOnAdOpened;
        // Called when the ad is closed.
        interstitial.OnAdClosed -= HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        interstitial.OnAdLeavingApplication -= HandleOnAdLeavingApplication;
    }
}
