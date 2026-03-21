using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Workshop47.Scripts.Game.MainMenu.Root;
using Workshop47.Scripts.Game.World.Root;
using Workshop47.Scripts.Utils;
using R3;
using Workshop47.Scripts.DI;
using Workshop47.Scripts.Game.Settings;
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
        }

        private void RunGame()
        {
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == Scenes.WORLD)
            {
                var enterParams = new WorldEnterParams();
                _coroutines.StartCoroutine(LoadAndStartWorld(enterParams));
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

        private IEnumerator LoadAndStartWorld(WorldEnterParams enterParams)
        {
            _uiRoot.ShowLoadingScreen();
            
            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.WORLD);

            yield return new WaitForSeconds(1);

            var sceneEntryPoint = Object.FindFirstObjectByType<WorldEntryPoint>();
            sceneEntryPoint.Run(_uiRoot, enterParams).Subscribe(worldExitParams =>
            {
                _coroutines.StartCoroutine(LoadAndStartMainMenu(worldExitParams.MainMenuEnterParams));
            });

            _uiRoot.HideLoadingScreen();
        }

        private IEnumerator LoadAndStartMainMenu(MainMenuEnterParams enterParams = null)
        {
            _uiRoot.ShowLoadingScreen();

            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.MAIN_MENU);

            var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
            sceneEntryPoint.Run(_uiRoot, enterParams).Subscribe(mainMenuExitParams =>
            {
                var targetSceneName = mainMenuExitParams.TargetSceneEnterParams.SceneName;
                if (targetSceneName == Scenes.WORLD)
                {
                    _coroutines.StartCoroutine(
                        LoadAndStartWorld(mainMenuExitParams.TargetSceneEnterParams.As<WorldEnterParams>()));
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