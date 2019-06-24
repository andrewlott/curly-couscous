using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Monetization;

public class AdSystem : BaseSystem {
#if UNITY_IOS
    private string _gameId = "3188551";
#elif UNITY_ANDROID
    private string _gameId = "3188550";
#endif
    private string _bannerPlacementId = "MainPlacement";
    private bool _testMode = true;

    public override void Start() {
		Pool.Instance.AddSystemListener(typeof(AdComponent), this);

        Monetization.Initialize(this._gameId, this._testMode);
        Advertisement.Initialize(this._gameId, this._testMode);
        //Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER); // default
        Advertisement.Banner.Hide();
    }

    public override void Stop() {
        Advertisement.Banner.Hide();
        Pool.Instance.RemoveSystemListener(typeof(AdComponent), this);
	}

	public override void Update() {

	}

	public override void OnComponentAdded(BaseComponent c) {
		if (c is AdComponent) {
            Controller().HandleCoroutine(ShowBannerWhenReady());
        }
	}

	public override void OnComponentRemoved(BaseComponent c) {
		if (c is AdComponent) {
            Advertisement.Banner.Hide();
        }
    }

    private IEnumerator ShowBannerWhenReady() {
        while (!Advertisement.IsReady(this._bannerPlacementId)) {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(this._bannerPlacementId);
    }
}
