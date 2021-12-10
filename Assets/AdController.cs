using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdController : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
string GameID = "4491595";
string SkippableID = "Interstitial_iOS";
string RewardedID = "Rewarded_iOS";
string BannerID = "Banner_iOS";
#else
    string GameID = "4491594";
    string SkippableID = "Interstitial_Android";
    string RewardedID = "Rewarded_Android";
    string BannerID = "Banner_Android";
#endif

    Action OnRewardAdComplete;

    void Start()
    {
        Advertisement.Initialize(GameID, true);
        Advertisement.AddListener(this);

        //  StartCoroutine(DelayedAd());
        PlayBannerAd();
    }

    void PlaySkippableAd()
    {
        if (Advertisement.IsReady(SkippableID))
        {
            Advertisement.Show(SkippableID);
        }
    }

    public void PlayRewardedAd(Action onAdComplete)
    {
        OnRewardAdComplete = onAdComplete;

        if (Advertisement.IsReady(RewardedID))
        {
            Advertisement.Show(RewardedID);
            
        }
    }

    void PlayBannerAd()
    {
        if (Advertisement.IsReady(BannerID))
        {
            Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
            Advertisement.Banner.Show(BannerID);
        } else
        {
            StartCoroutine(CheckIfInitialized());
        }
    }
    
    void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    IEnumerator DelayedAd()
    {
        yield return new WaitForSeconds(3);
       // PlayRewardedAd();
        PlayBannerAd();
    }

    IEnumerator CheckIfInitialized()
    {
        yield return new WaitForSeconds(2);
        PlayBannerAd();
    }

    public void OnUnityAdsReady(string placementId)
    {
        
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("Error : " + message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Start : " + placementId);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == RewardedID && showResult == ShowResult.Finished)
        {
            GameController.control.Console("Sucess : " + placementId);
            OnRewardAdComplete.Invoke();
        } else { Debug.Log("fail"); }
    }
}
