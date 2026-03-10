using UnityEngine;

[CreateAssetMenu(
    fileName = "Config_AdMobAds",
    menuName = "Runner/Configs/AdMob Ads Config")]
public class AdMobAdsConfig : ScriptableObject
{
    [field: SerializeField] public bool UseTestAdUnitIds { get; private set; } = true;

    [field: SerializeField] public string AndroidRewardedAdUnitId { get; private set; }
    [field: SerializeField] public string IosRewardedAdUnitId { get; private set; }
}