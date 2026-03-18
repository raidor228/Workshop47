using R3;
using UnityEngine;
using Workshop47.Scripts.Game.MainMenu.Root;
using Workshop47.Scripts.Game.Root;
using Workshop47.Scripts.Game.World.Root.View;

namespace Workshop47.Scripts.Game.World.Root
{
    public class WorldEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIWorldRootBinder _sceneUIRootPrefab;

        public Observable<WorldExitParams> Run(UIRootView uiRoot, WorldEnterParams enterParams)
        {
            var uiScene = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiScene.gameObject);

            var exitSceneSignalSubj = new Subject<Unit>();
            uiScene.Bind(exitSceneSignalSubj);

            var mainMenuEnterParams = new MainMenuEnterParams();
            var exitParams = new WorldExitParams(mainMenuEnterParams);
            var exitToMainMenuSceneSignal = exitSceneSignalSubj.Select(_ => exitParams);

            return exitToMainMenuSceneSignal;
        }
    }
}