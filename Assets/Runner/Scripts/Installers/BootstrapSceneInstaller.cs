using Zenject;

public class BootstrapSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ProjectStartupSystem>().AsSingle().NonLazy();
    }
}