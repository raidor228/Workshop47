using System.Collections;
using Melador.PlayerController.CameraController.Settings;
using Melador.PlayerController.MovementController.Settings;
using Melador.PlayerInput;
using UnityEngine;
using UnityEngine.SceneManagement;
using Workshop47.Scripts.Game.MainMenu.Root;
using Workshop47.Scripts.Utils;
using R3;
using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Settings;
using Workshop47.Scripts.Game.Settlement.Root;
using Workshop47.Scripts.Game.State;

namespace Workshop47.Scripts.Game.Root
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;
        private readonly Coroutines _coroutines;
        private readonly UIRootView _uiRoot;
        private readonly DIContainer _rootContainer = new();
        private DIContainer _cachedSceneContainer;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutostartGame()
        {
            _instance = new GameEntryPoint();
            _instance.RunGame();
        }

        private GameEntryPoint()
        {
            _coroutines = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
            Object.DontDestroyOnLoad(_coroutines.gameObject);

            var prefabUIRoot = Resources.Load<UIRootView>("UIRoot");
            _uiRoot = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(_uiRoot.gameObject);
            _rootContainer.RegisterInstance(_uiRoot);
            
            var settingsProvider = new SettingsProvider();
            _rootContainer.RegisterInstance<ISettingsProvider>(settingsProvider);

            var gameStateProvider = new JsonGameStateProvider();
            gameStateProvider.LoadSettingsState();
            _rootContainer.RegisterInstance<IGameStateProvider>(gameStateProvider);

            var playerSettings = Resources.Load<PlayerSettings>("Settings/Player/PlayerSettings");
            var cameraSettings = Resources.Load<CameraSettings>("Settings/Player/CameraSettings");
            var playerInputProvider = new PlayerInputProvider(playerSettings, cameraSettings);
            _rootContainer.RegisterInstance(playerInputProvider);
        }

        private async void RunGame()
        {
            await _rootContainer.Resolve<ISettingsProvider>().LoadGameSettings();
            
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == Scenes.SETTLEMENT)
            {
                var enterParams = new SettlementEnterParams(0);
                _coroutines.StartCoroutine(LoadAndStartSettlement(enterParams));
                return;
            }

            if (sceneName == Scenes.MAIN_MENU)
            {
                _coroutines.StartCoroutine(LoadAndStartMainMenu());
            }

            if (sceneName != Scenes.BOOT)
            {
                return;
            }
#endif

            _coroutines.StartCoroutine(LoadAndStartMainMenu());
        }

        private IEnumerator LoadAndStartSettlement(SettlementEnterParams enterParams)
        {
            _uiRoot.ShowLoadingScreen();
            _cachedSceneContainer?.Dispose();
            
            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.SETTLEMENT);

            yield return new WaitForSeconds(1);

            var isGameStateLoaded = false;
            _rootContainer.Resolve<IGameStateProvider>().LoadGameState().Subscribe(_ => isGameStateLoaded = true);
            yield return new WaitUntil(() => isGameStateLoaded);
            
            var sceneEntryPoint = Object.FindFirstObjectByType<SettlementEntryPoint>();
            var settlementContainer = _cachedSceneContainer = new DIContainer(_rootContainer);
            sceneEntryPoint.Run(settlementContainer, enterParams).Subscribe(settlementExitParams =>
            {
                _coroutines.StartCoroutine(LoadAndStartMainMenu(settlementExitParams.MainMenuEnterParams));
            });

            _uiRoot.HideLoadingScreen();
        }

        private IEnumerator LoadAndStartMainMenu(MainMenuEnterParams enterParams = null)
        {
            _uiRoot.ShowLoadingScreen();
            _cachedSceneContainer?.Dispose();
            
            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.MAIN_MENU);

            var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
            var mainMenuContainer = _cachedSceneContainer = new DIContainer(_rootContainer);
            sceneEntryPoint.Run(mainMenuContainer, enterParams).Subscribe(mainMenuExitParams =>
            {
                var targetSceneName = mainMenuExitParams.TargetSceneEnterParams.SceneName;
                if (targetSceneName == Scenes.SETTLEMENT)
                {
                    _coroutines.StartCoroutine(
                        LoadAndStartSettlement(mainMenuExitParams.TargetSceneEnterParams.As<SettlementEnterParams>()));
                }
            });

            _uiRoot.HideLoadingScreen();
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}