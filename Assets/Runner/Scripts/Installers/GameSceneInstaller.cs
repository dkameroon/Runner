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
    [SerializeField] private AudioConfig _audioConfig;

    [Header("Debug Overlay")]
    [SerializeField] private DebugOverlayView _debugOverlayView;

    [Header("UI")]
    [SerializeField] private MainMenuWindow _mainMenuWindow;
    [SerializeField] private AuthWindow _authWindow;
    [SerializeField] private LeaderboardWindow _leaderboardWindow;
    [SerializeField] private LeaderboardEntryElement _leaderboardEntryElementPrefab;
    [SerializeField] private DefeatPopup _defeatPopupPrefab;
    [SerializeField] private PausePopup _pausePopupPrefab;
    [SerializeField] private SettingsPopup _settingsPopupPrefab;
    [SerializeField] private GameHUDView _gameHudView;
    [SerializeField] private PopupCanvasRootView _popupCanvasRootView;
    [SerializeField] private PauseButtonView _pauseButtonView;

    [Header("Audio")]
    [SerializeField] private AudioPlayerView _audioPlayerView;

    public override void InstallBindings()
    {
        BindConfigs();
        BindSceneViews();
        BindInput();
        BindCore();
        BindFirebase();
        BindFactories();
        BindGameplay();
        BindAudio();
        BindUI();
        BindDebug();
    }

    private void BindConfigs()
    {
        Container.BindInstance(_runnerGameConfig).AsSingle();
        Container.BindInstance(_obstacleSpawnConfig).AsSingle();
        Container.BindInstance(_obstaclePrefabsConfig).AsSingle();
        Container.BindInstance(_worldGenerationConfig).AsSingle();
        Container.BindInstance(_obstacleDifficultyConfig).AsSingle();
        Container.BindInstance(_debugOverlayConfig).AsSingle();
        Container.BindInstance(_audioConfig).AsSingle();
    }

    private void BindSceneViews()
    {
        Container.Bind<PlayerView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerCollisionView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerScoreUpdateView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CameraTargetFollowView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerInputView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerAnimatorView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayerHitboxView>().FromComponentInHierarchy().AsSingle();

        Container.Bind<MainMenuWindow>().FromInstance(_mainMenuWindow).AsSingle();
        Container.Bind<AuthWindow>().FromInstance(_authWindow).AsSingle();
        Container.Bind<LeaderboardWindow>().FromInstance(_leaderboardWindow).AsSingle();
        Container.Bind<GameHUDView>().FromInstance(_gameHudView).AsSingle();
        Container.Bind<PopupCanvasRootView>().FromInstance(_popupCanvasRootView).AsSingle();
        Container.Bind<PauseButtonView>().FromInstance(_pauseButtonView).AsSingle();
        Container.Bind<AudioPlayerView>().FromInstance(_audioPlayerView).AsSingle();

#if UNITY_EDITOR
        Container.Bind<EditorDebugRestartInputView>().FromComponentInHierarchy().AsSingle().NonLazy();
#endif
    }

    private void BindInput()
    {
#if UNITY_EDITOR
        Container.Bind<IPlayerInputStrategy>().To<EditorKeyboardInputStrategy>().AsSingle();
#else
        Container.Bind<IPlayerInputStrategy>().To<MobileSwipeInputStrategy>().AsSingle();
#endif
    }

    private void BindCore()
    {
        Container.Bind<SceneHierarchyService>().AsSingle().NonLazy();
        Container.Bind<GameplaySessionService>().AsSingle();
        Container.Bind<ObstacleRegistryService>().AsSingle();

        Container.Bind<ObstacleDifficultyProvider>().AsSingle();
        Container.Bind<ObstacleWavePatternProvider>().AsSingle();
        Container.Bind<LanePositionProvider>().AsSingle();
        Container.Bind<ObstaclePatternSpawnService>().AsSingle();

        Container.Bind<PlayerScoreResetService>().AsSingle();
        Container.Bind<GameplayRestartService>().AsSingle();
        Container.Bind<PlayerContinueService>().AsSingle();
        Container.Bind<PlayerGameStateService>().AsSingle();

        Container.BindInterfacesAndSelfTo<ObstaclePoolService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<WorldSegmentPoolService>().AsSingle().NonLazy();
        Container.Bind<GamePopupService>().AsSingle();
    }

    private void BindFirebase()
    {
        Container.Bind<FirebaseServiceProvider>().AsSingle();
        Container.Bind<FirebaseAuthErrorMessageProvider>().AsSingle();
        Container.Bind<FirebaseFirestoreErrorMessageProvider>().AsSingle();

        Container.Bind<IAuthenticationService>().To<FirebaseAuthenticationService>().AsSingle();
        Container.BindInterfacesAndSelfTo<FirestoreBootstrapProbe>().AsSingle().NonLazy();

        Container.Bind<ILeaderboardService>().To<FirebaseLeaderboardService>().AsSingle();
        Container.BindInterfacesAndSelfTo<LeaderboardSubmitService>().AsSingle().NonLazy();
    }

    private void BindFactories()
    {
        Container.BindInstance(_defeatPopupPrefab).AsSingle();
        Container.BindInstance(_pausePopupPrefab).AsSingle();
        Container.BindInstance(_settingsPopupPrefab).AsSingle();
        Container.BindInstance(_leaderboardEntryElementPrefab).AsSingle();

        Container.Bind<LeaderboardEntryElementFactory>().AsSingle();
        Container.Bind<PlayerStateFactory>().AsSingle();

        Container.BindInterfacesAndSelfTo<ObstacleFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<PopupFactory>().AsSingle();
    }

    private void BindGameplay()
    {
        Container.BindInterfacesAndSelfTo<SpeedSystem>().AsSingle().NonLazy();

        Container.Bind<PlayerScoreSystem>().AsSingle();
        Container.Bind<PlayerRespawnSystem>().AsSingle();
        Container.Bind<PlayerStateContextModel>().AsSingle();

        Container.BindInterfacesAndSelfTo<PlayerMovementSystem>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PlayerStateMachineSystem>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<RunnerWorldSpawnSystem>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ObstacleSpawnSystem>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ObstacleCleanupSystem>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameFlowSystem>().AsSingle().NonLazy();
    }

    private void BindAudio()
    {
        Container.Bind<AudioSettingsService>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameAudioService>().AsSingle().NonLazy();
    }

    private void BindUI()
    {
        Container.Bind<AuthInputValidationService>().AsSingle();
        Container.Bind<AuthUIScreenService>().AsSingle();
        Container.Bind<LeaderboardPlayerBestScoreProvider>().AsSingle();

        Container.BindInterfacesAndSelfTo<GameUIService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameHUDService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AuthFlowService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<StartupAuthFlowSystem>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<MainMenuFlowService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LeaderboardFlowService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<DefeatContinueFlowService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PauseFlowService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SettingsFlowService>().AsSingle().NonLazy();
    }

    private void BindDebug()
    {
        Container.Bind<DebugOverlayView>().FromInstance(_debugOverlayView).AsSingle();
        Container.BindInterfacesTo<DebugOverlaySystem>().AsSingle();
    }
}