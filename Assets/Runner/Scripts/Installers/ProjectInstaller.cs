using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [Header("Configs")]
    [SerializeField] private AdMobAdsConfig _adMobAdsConfig;

    public override void InstallBindings()
    {
        Container.Bind<SceneLoaderService>().AsSingle();

        Container.BindInstance(_adMobAdsConfig).AsSingle();

        Container.Bind<IAdsService>().To<AdMobRewardedAdsService>().AsSingle();

        Container.Bind<FirebaseBootstrapService>().AsSingle();
        Container.Bind<AdsBootstrapService>().AsSingle();
    }
}