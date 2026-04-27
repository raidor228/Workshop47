using UnityEngine;

namespace Workshop47.Scripts.Fsm
{
    public abstract class FsmState
    {
        protected readonly Scripts.Fsm.Fsm Fsm;

        public FsmState(Scripts.Fsm.Fsm fsm)
        {
            Fsm = fsm;
        }

        public virtual void Enter() { }
        public virtual void LogicUpdate() { }
        public virtual void PhysicsUpdate() { }
        public virtual void LateUpdate() { }
        public virtual void OnTriggerEnter(Collider collider) { }
        public virtual void OnTriggerStay(Collider other) { }
        public virtual void OnTriggerExit(Collider collider) { }
        public virtual void OnControllerColliderHit(ControllerColliderHit hit) { }
        public virtual void Exit() { }
    }
}