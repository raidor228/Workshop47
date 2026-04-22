using Melador.PlayerController.MovementController.States.Root;
using UnityEngine;

namespace Melador.PlayerController.MovementController.States.Handlers
{
    public class AnimatorHandler
    {
        private readonly Animator _animator;
        
        private readonly MovementStateContext _stateContext;
        
        public AnimatorHandler(MovementStateContext stateContext, Animator animator)
        {
            _stateContext = stateContext;
            _animator = animator;
        }

        public void SetTrigger(string trigger)
        {
            _animator.SetTrigger(trigger);
        }

        public void SetFloat(string parameter, float value)
        {
            _animator.SetFloat(parameter, value);
        }
    }
}