using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private RunnerGameConfig _runnerGameConfig;
    [SerializeField] private ObstacleSpawnConfig _obstacleSpawnConfig;
    [SerializeField] private ObstaclePrefabsConfig _obstaclePrefabsConfig;
    [SerializeField] private WorldGenerationConfig _worldGenerationConfig;

    public override void InstallBindings()
    {
        Container.BindInstance(_runnerGameConfig).AsSingle();
        Container.BindInstance(_obstacleSpawnConfig).AsSingle();
        Container.BindInstance(_obstaclePrefabsConfig).AsSingle();
        Container.BindInstance(_worldGenerationConfig).AsSingle();
        
        Container.Bind<PlayerView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerCollisionView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerScoreUpdateView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<CameraTargetFollowView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerInputView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerAnimatorView>().FromComponentInHierarchy().AsSingle().NonLazy();
        
        Container.Bind<PlayerHitboxView>().FromComponentInHierarchy().AsSingle();

        Container.Bind<ObstacleRegistryService>().AsSingle();

        Container.BindInterfacesAndSelfTo<ObstaclePoolService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ObstacleFactory>().AsSingle();

        Container.BindInterfacesAndSelfTo<RunnerWorldSpawnSystem>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<WorldSegmentPoolService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ObstacleSpawnSystem>().AsSingle().NonLazy();
        
        Container.BindInterfacesAndSelfTo<ObstacleCleanupSystem>().AsSingle().NonLazy();
        
        Container.BindInterfacesAndSelfTo<SpeedSystem>().AsSingle().NonLazy();
        Container.Bind<PlayerScoreSystem>().AsSingle();
        Container.Bind<PlayerRespawnSystem>().FromMethod(CreateRespawnSystem).AsSingle();
        
        Container.Bind<PlayerStateContextModel>().FromMethod(_ =>
                new PlayerStateContextModel(
                    Container.Resolve<PlayerView>(),
                    Container.Resolve<PlayerAnimatorView>(),
                    Container.Resolve<PlayerHitboxView>(),
                    Container.Resolve<RunnerGameConfig>()))
            .AsSingle();
        
        Container.BindInterfacesAndSelfTo<PlayerStateMachineSystem>().AsSingle().NonLazy();
        
#if UNITY_EDITOR
        Container.Bind<EditorDebugRestartInputView>().FromComponentInHierarchy().AsSingle().NonLazy();
#endif
        Container.Bind<SceneHierarchyService>().AsSingle().NonLazy();
    }

    private PlayerRespawnSystem CreateRespawnSystem(InjectContext _)
    {
        PlayerView playerView = Container.Resolve<PlayerView>();
        CameraTargetFollowView cameraFollow = Container.Resolve<CameraTargetFollowView>();
        PlayerScoreSystem scoreSystem = Container.Resolve<PlayerScoreSystem>();

        var restartables = Container.Resolve<List<IRestartable>>();

        return new PlayerRespawnSystem(
            playerView.transform,
            playerView,
            cameraFollow,
            scoreSystem,
            restartables);
    }
}