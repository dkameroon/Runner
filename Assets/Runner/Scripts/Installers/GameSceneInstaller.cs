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
    [SerializeField] private ObstacleDifficultyConfig _obstacleDifficultyConfig;
    
    [Header("Debug Overlay")]
    [SerializeField] private DebugOverlayView _debugOverlayView;
    
    [Header("UI")]
    [SerializeField] private MainMenuWindow _mainMenuWindow;
    [SerializeField] private AuthWindow _authWindow;
    [SerializeField] private LeaderboardWindow _leaderboardWindow;
    [SerializeField] private LeaderboardEntryElement _leaderboardEntryElementPrefab;
    [SerializeField] private DefeatPopup _defeatPopupPrefab;
    [SerializeField] private PausePopup _pausePopupPrefab;
    [SerializeField] private GameHUDView _gameHudView;
    [SerializeField] private PopupCanvasRootView _popupCanvasRootView;
    [SerializeField] private PauseButtonView _pauseButtonView;

    public override void InstallBindings()
    {
        BindConfigs();
        BindViews();
        BindInput();
        BindServices();
        BindUIServices();
        BindFactories();
        BindSystems();
        BindDebug();
    }
    
    private void BindInput()
    {
#if UNITY_EDITOR
        Container.Bind<IPlayerInputStrategy>().To<EditorKeyboardInputStrategy>().AsSingle();
#else
        Container.Bind<IPlayerInputStrategy>().To<MobileSwipeInputStrategy>().AsSingle();
#endif
    }

    private void BindConfigs()
    {
        Container.BindInstance(_runnerGameConfig).AsSingle();
        Container.BindInstance(_obstacleSpawnConfig).AsSingle();
        Container.BindInstance(_obstaclePrefabsConfig).AsSingle();
        Container.BindInstance(_worldGenerationConfig).AsSingle();
        Container.BindInstance(_obstacleDifficultyConfig).AsSingle();
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
        Container.Bind<AuthWindow>().FromInstance(_authWindow).AsSingle();
        Container.Bind<LeaderboardWindow>().FromInstance(_leaderboardWindow).AsSingle();
        Container.Bind<GameHUDView>().FromInstance(_gameHudView).AsSingle();
        Container.Bind<PopupCanvasRootView>().FromInstance(_popupCanvasRootView).AsSingle();
        Container.Bind<PauseButtonView>().FromInstance(_pauseButtonView).AsSingle();
        
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
        Container.BindInterfacesAndSelfTo<PopupService>().AsSingle();

        Container.Bind<IAuthenticationService>().To<FirebaseAuthenticationService>().AsSingle();
        Container.BindInterfacesAndSelfTo<FirestoreBootstrapProbe>().AsSingle().NonLazy();
        Container.Bind<ILeaderboardService>().To<FirebaseLeaderboardService>().AsSingle();

        Container.BindInterfacesAndSelfTo<LeaderboardSubmitService>().AsSingle().NonLazy();
    }
    
    private void BindUIServices()
    {
        Container.BindInterfacesAndSelfTo<GameUIService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameHUDService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AuthFlowService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<StartupAuthFlowSystem>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<MainMenuFlowService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LeaderboardFlowService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DefeatContinueFlowService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PauseFlowService>().AsSingle().NonLazy();
    }

    private void BindFactories()
    {
        Container.BindInterfacesAndSelfTo<ObstacleFactory>().AsSingle();
        Container.BindInstance(_defeatPopupPrefab).AsSingle();
        Container.BindInstance(_pausePopupPrefab).AsSingle();
        Container.BindInstance(_leaderboardEntryElementPrefab).AsSingle();
        Container.BindInterfacesAndSelfTo<PopupFactory>().AsSingle();
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