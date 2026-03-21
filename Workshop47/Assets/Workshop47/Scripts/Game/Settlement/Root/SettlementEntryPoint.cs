using R3;
using UnityEngine;
using Workshop47.Scripts.Game.MainMenu.Root;
using Workshop47.Scripts.Game.Root;
using Workshop47.Scripts.Game.Settlement.Root.View;

namespace Workshop47.Scripts.Game.Settlement.Root
{
    public class SettlementEntryPoint : MonoBehaviour
    {
        [SerializeField] private UISettlementRootBinder _sceneUIRootPrefab;

        public Observable<SettlementExitParams> Run(UIRootView uiRoot, SettlementEnterParams enterParams)
        {
            var uiScene = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiScene.gameObject);

            var exitSceneSignalSubj = new Subject<Unit>();
            uiScene.Bind(exitSceneSignalSubj);

            var mainMenuEnterParams = new MainMenuEnterParams();
            var exitParams = new SettlementExitParams(mainMenuEnterParams);
            var exitToMainMenuSceneSignal = exitSceneSignalSubj.Select(_ => exitParams);

            return exitToMainMenuSceneSignal;
        }
    }
}