using System;
using System.Linq;
using R3;
using TMPro;
using UnityEngine;
using Workshop47.Scripts.MVVM.UI;

namespace Workshop47.Scripts.Game.Gameplay.View.UI.ScreenGameplay
{
    public class ScreenGameplayBinder : WindowBinder<ScreenGameplayViewModel>
    {
        [SerializeField] private CanvasGroup _cgInteractions;
        [SerializeField] private TMP_Text _txtInteractionTitle;
        [SerializeField] private GameObject _interactionLeftArrow;
        [SerializeField] private GameObject _interactionRightArrow;

        private IDisposable _disposable;
        
        protected override void OnBind(ScreenGameplayViewModel viewModel)
        {
            _disposable = viewModel.SelectedInteractable.Subscribe(OnSelectedInteractable);
        }
        
        private void OnSelectedInteractable(int selectedInteractableIndex)
        {
            var interactables = ViewModel.Interactables;
            int count = interactables.Count;

            if (selectedInteractableIndex < 0 || count == 0)
            {
                _cgInteractions.alpha = 0f;
                _interactionLeftArrow.SetActive(false);
                _interactionRightArrow.SetActive(false);
                return;
            }

            _cgInteractions.alpha = 1f;

            var selected = interactables.ElementAt(selectedInteractableIndex);
            _txtInteractionTitle.text = selected.Title;

            bool hasMultiple = count > 1;

            _interactionLeftArrow.SetActive(hasMultiple && selectedInteractableIndex > 0);
            _interactionRightArrow.SetActive(hasMultiple && selectedInteractableIndex < count - 1);
        }

        public override void Close()
        {
            _disposable.Dispose();
            base.Close();
        }
    }
}