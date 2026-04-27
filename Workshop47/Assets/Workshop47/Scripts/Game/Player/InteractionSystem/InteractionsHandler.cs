using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;
using Workshop47.Scripts.Game.Gameplay.View.Interactable;
using Workshop47.Scripts.Game.Player.InteractionSystem.Settings;

namespace Workshop47.Scripts.Game.Player.InteractionSystem
{
    public class InteractionsHandler
    {
        private readonly Transform _root;
        private readonly InteractionsSettings _interactionsSettings;
        
        private readonly Subject<List<IInteractable>> _onInteractablesOverlap;
        
        private float _timer;
        
        public InteractionsHandler(Transform root, InteractionsSettings interactionsSettings, 
            Subject<List<IInteractable>> onInteractablesOverlap)
        {
            _root = root;
            _interactionsSettings = interactionsSettings;
            _onInteractablesOverlap = onInteractablesOverlap;
        }

        public void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _interactionsSettings.OverlapFrequency)
            {
                _timer = 0f;
                
                var overlapped = GetInteractables();
                _onInteractablesOverlap.OnNext(overlapped);
            }
        }

        private List<IInteractable> GetInteractables()
        {
            Vector3 rootPosition = _root.position;
            var colliders = Physics.OverlapSphere(rootPosition, _interactionsSettings.InteractionsRadius,
                _interactionsSettings.InteractionLayer);

            HashSet<IInteractable> interactables = new();
            for (int i = 0; i < colliders.Length; i++)
            {
                var hit = colliders[i];
                if (!hit.TryGetComponent<IInteractable>(out var interactable))
                {
                    continue;
                }

                interactables.Add(interactable);
            }

            return interactables.OrderBy(i =>
            {
                float da = (i.RootTransform.position - rootPosition).sqrMagnitude;
                return da;
            }).ToList();
        }
    }
}