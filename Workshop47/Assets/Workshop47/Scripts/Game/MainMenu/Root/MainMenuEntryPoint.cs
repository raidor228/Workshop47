using R3;
using UnityEngine;
using Workshop47.Scripts.Game.MainMenu.Root.View;
using Workshop47.Scripts.Game.Root;
using Workshop47.Scripts.Game.World.Root;

namespace Workshop47.Scripts.Game.MainMenu.Root
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIMainMenuRootBinder _sceneUIRootPrefab;

        public Observable<MainMenuExitParams> Run(UIRootView uiRoot, MainMenuEnterParams enterParams)
        {
            var uiScene = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiScene.gameObject);
            
            var exitSignalSubj = new Subject<Unit>();
            uiScene.Bind(exitSignalSubj);

            var worldEnterParams = new WorldEnterParams();
            var mainMenuExitParams = new MainMenuExitParams(worldEnterParams);
            var exitToGameplaySceneSignal = exitSignalSubj.Select(_ => mainMenuExitParams);
            
            return exitToGameplaySceneSignal;
        }   
    }
}