using UnityEngine;
using Zenject;

public class BootstrapSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log("BootstrapSceneInstaller: InstallBindings");

        Container.BindInterfacesAndSelfTo<ProjectStartupSystem>().AsSingle().NonLazy();
    }
}