using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log("ProjectInstaller: InstallBindings");

        Container.Bind<SceneLoaderService>().AsSingle();
    }
}