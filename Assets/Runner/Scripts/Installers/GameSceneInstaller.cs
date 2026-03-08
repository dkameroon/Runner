using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [Header("Configs")]
    [SerializeField] private RunnerGameConfig _runnerGameConfig;
    [SerializeField] private ObstacleSpawnConfig _obstacleSpawnConfig;
    [SerializeField] private ObstaclePrefabsConfig _obstaclePrefabsConfig;
    [SerializeField] private WorldGenerationConfig _worldGenerationConfig;
    [SerializeField] private DebugOverlayConfig _debugOverlayConfig;
    
    [Header("Debug Overlay")]
    [SerializeField] private DebugOverlayView _debugOverlayView;
    
    [Header("UI")]
    [SerializeField] private MainMenuWindow _mainMenuWindow;
    [SerializeField] private DefeatPopup _defeatPopup;
    [SerializeField] private GameHUDView _gameHudView;

    public override void InstallBindings()
    {
        BindConfigs();
        BindViews();
        BindServices();
        BindUIServices();
        BindFactories();
        BindSystems();
        BindDebug();
    }

    private void BindConfigs()
    {
        Container.BindInstance(_runnerGameConfig).AsSingle();
        Container.BindInstance(_obstacleSpawnConfig).AsSingle();
        Container.BindInstance(_obstaclePrefabsConfig).AsSingle();
        Container.BindInstance(_worldGenerationConfig).AsSingle();
    }

    private void BindViews()
    {
        Container.Bind<PlayerView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerCollisionView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerScoreUpdateView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<CameraTargetFollowView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerInputView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerAnimatorView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerHitboxView>().FromComponentInHierarchy().AsSingle();
        
        Container.Bind<MainMenuWindow>().FromInstance(_mainMenuWindow).AsSingle();
        Container.Bind<DefeatPopup>().FromInstance(_defeatPopup).AsSingle();
        Container.Bind<GameHUDView>().FromInstance(_gameHudView).AsSingle();
        
#if UNITY_EDITOR
        Container.Bind<EditorDebugRestartInputView>().FromComponentInHierarchy().AsSingle().NonLazy();
#endif
        
    }

    private void BindServices()
    {
        Container.Bind<SceneHierarchyService>().AsSingle().NonLazy();
        Container.Bind<ObstacleRegistryService>().AsSingle();

        Container.BindInterfacesAndSelfTo<ObstaclePoolService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<WorldSegmentPoolService>().AsSingle().NonLazy();
        Container.Bind<GameplaySessionService>().AsSingle();
    }
    
    private void BindUIServices()
    {
        Container.BindInterfacesAndSelfTo<GameUIService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameHUDService>().AsSingle().NonLazy();
    }

    private void BindFactories()
    {
        Container.BindInterfacesAndSelfTo<ObstacleFactory>().AsSingle();
    }

    private void BindSystems()
    {
        Container.BindInterfacesAndSelfTo<SpeedSystem>().AsSingle().NonLazy();

        Container.Bind<PlayerScoreSystem>().AsSingle();
        Container.Bind<PlayerRespawnSystem>().AsSingle();

        Container.Bind<PlayerStateContextModel>()
            .FromMethod(_ => new PlayerStateContextModel(
                Container.Resolve<PlayerView>(),
                Container.Resolve<PlayerAnimatorView>(),
                Container.Resolve<PlayerHitboxView>(),
                Container.Resolve<RunnerGameConfig>()))
            .AsSingle();

        Container.BindInterfacesAndSelfTo<PlayerStateMachineSystem>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<RunnerWorldSpawnSystem>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ObstacleSpawnSystem>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ObstacleCleanupSystem>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameFlowSystem>().AsSingle().NonLazy();
    }

    private void BindDebug()
    {
        Container.Bind<DebugOverlayView>()
            .FromInstance(_debugOverlayView)
            .AsSingle();

        Container.Bind<DebugOverlayConfig>()
            .FromInstance(_debugOverlayConfig)
            .AsSingle();

        Container.BindInterfacesTo<DebugOverlaySystem>()
            .AsSingle();
    }
}