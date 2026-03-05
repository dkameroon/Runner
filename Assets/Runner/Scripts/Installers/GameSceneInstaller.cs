using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private RunnerGameConfig _runnerGameConfig;

    public override void InstallBindings()
    {
        Container.BindInstance(_runnerGameConfig).AsSingle();
        
        Container.Bind<PlayerScoreSystem>().AsSingle();
        Container.Bind<PlayerRespawnSystem>().FromMethod(CreateRespawnSystem).AsSingle();
        
        Container.Bind<PlayerView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerCollisionView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerScoreUpdateView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<CameraTargetFollowView>().FromComponentInHierarchy().AsSingle().NonLazy();
        
        Container.BindInterfacesAndSelfTo<SpeedSystem>().AsSingle().NonLazy();
        
        Container.Bind<EditorLaneInputView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<EditorRestartInputView>().FromComponentInHierarchy().AsSingle().NonLazy();
        
        Container.Bind<PlayerStateContextModel>().FromMethod(_ =>
                new PlayerStateContextModel(
                    Container.Resolve<PlayerView>(),
                    Container.Resolve<RunnerGameConfig>()))
            .AsSingle();

        Container.BindInterfacesAndSelfTo<PlayerStateMachineSystem>().AsSingle().NonLazy();
    }

    private PlayerRespawnSystem CreateRespawnSystem(InjectContext _)
    {
        PlayerView playerView = Container.Resolve<PlayerView>();
        CameraTargetFollowView cameraFollow = Container.Resolve<CameraTargetFollowView>();
        
        PlayerScoreSystem scoreSystem = Container.Resolve<PlayerScoreSystem>();

        return new PlayerRespawnSystem(
            playerView.transform,
            playerView,
            cameraFollow,
            scoreSystem);
    }
}