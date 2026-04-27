using System;
using System.Collections.Generic;
using System.Linq;
using Melador.PlayerInput.Modules;
using ObservableCollections;
using Workshop47.Scripts.Game.Gameplay.View.Interactable;
using Workshop47.Scripts.MVVM.UI;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Workshop47.Scripts.Game.Gameplay.View.UI.ScreenGameplay
{
    public class ScreenGameplayViewModel : WindowViewModel
    {
        public override string Id => "ScreenGameplay";

        public ReadOnlyReactiveProperty<int> SelectedInteractable => _selectedInteractable;
        private readonly ReactiveProperty<int> _selectedInteractable = new(-1);
        
        public IObservableCollection<IInteractable> Interactables => _interactables;
        private readonly ObservableList<IInteractable> _interactables = new();
        
        private readonly Subject<List<IInteractable>> _onInteractablesOverlap;
        private readonly InteractionsInput _interactionsInput;
        private readonly GameplayUIManager _uiManager;
        private readonly IDisposable _disposable;
        
        public ScreenGameplayViewModel(GameplayUIManager uiManager, InteractionsInput interactionsInput,
            Subject<List<IInteractable>> onInteractablesOverlap)
        {
            _uiManager = uiManager;
            _interactionsInput = interactionsInput;
            _onInteractablesOverlap = onInteractablesOverlap;

            _disposable = onInteractablesOverlap.Subscribe(OnInteractablesOverlap);
            
            _interactionsInput.NextInteractionAction.performed += NextInteractionActionOnperformed;
            _interactionsInput.PreviousInteractionAction.performed += PreviousInteractionActionOnperformed;
            _interactionsInput.InteractAction.performed += InteractActionOnperformed;
        }

        public void SelectNext()
        {
            _selectedInteractable.Value++;
            if (_selectedInteractable.CurrentValue >= _interactables.Count)
            {
                _selectedInteractable.Value = 0;
            }
        }

        public void SelectPrevious()
        {
            _selectedInteractable.Value--;
            if (_selectedInteractable.CurrentValue < 0)
            {
                _selectedInteractable.Value = _interactables.Count - 1;
            }
        }
        
        private void OnInteractablesOverlap(List<IInteractable> interactables)
        {
            if (_selectedInteractable.CurrentValue < _interactables.Count && 
                _selectedInteractable.CurrentValue != -1)
            {
                int selectedUniqueId = _interactables[_selectedInteractable.CurrentValue].UniqueId;
                
                _interactables.Clear();
                _interactables.AddRange(interactables);
                
                var lastSelected = _interactables.FirstOrDefault(i => i.UniqueId == selectedUniqueId);
                if (lastSelected == null)
                {
                    if (_interactables.Count == 0)
                    {
                        _selectedInteractable.Value = -1;
                    }
                    else
                    {
                        _selectedInteractable.Value = 0;
                    }
                }
                else
                {
                    int lastSelectedInteractableIndex = _interactables.IndexOf(lastSelected);
                    _selectedInteractable.Value = lastSelectedInteractableIndex;
                }

                _selectedInteractable.ForceNotify();
                return;
            }
            
            _interactables.Clear();
            _interactables.AddRange(interactables);
            if (_interactables.Count == 0)
            {
                _selectedInteractable.Value = -1;
            }
            else
            {
                _selectedInteractable.Value = 0;
            }
            
            _selectedInteractable.ForceNotify();
        }
        
        private void InteractActionOnperformed(InputAction.CallbackContext obj)
        {
            if (_selectedInteractable.CurrentValue == -1)
            {
                return;
            }

            var selectedInteractable = _interactables[_selectedInteractable.CurrentValue];
            selectedInteractable.Interact();
        }

        private void PreviousInteractionActionOnperformed(InputAction.CallbackContext obj)
        {
            SelectPrevious();
        }

        private void NextInteractionActionOnperformed(InputAction.CallbackContext obj)
        {
            SelectNext();
        }
        
        public override void Dispose()
        {
            _interactionsInput.NextInteractionAction.performed -= NextInteractionActionOnperformed;
            _interactionsInput.PreviousInteractionAction.performed -= PreviousInteractionActionOnperformed;
            _interactionsInput.InteractAction.performed -= InteractActionOnperformed;
            
            _disposable.Dispose();
        }
    }
}