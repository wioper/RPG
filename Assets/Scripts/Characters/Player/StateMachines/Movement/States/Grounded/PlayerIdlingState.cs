
using Unity.VisualScripting;
using UnityEngine;

namespace GenShinImpactMovementSystem
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        private PlayerIdleData idleData;
        protected internal PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
            idleData = movementData.IdleData;
        }

        #region IState Methods

        public override void Enter() {
            stateMachine.ReusableData.MovementSpeedModifier = 0f;//速度调节器数值修改为0
            
            stateMachine.ReusableData.BackwardsCameraRecenteringData = idleData.BackwardsCameraRecenteringData;
            
            base.Enter();

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StationaryForce;
            ResetVelocity();

        }

        public override void Update() {
            base.Update();
            if (stateMachine.ReusableData.MovementInput==Vector2.zero) {
               return; 
            }

            OnMove();
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();

            if (!IsMovingHorizontally()) {
                return;
            }
            ResetVelocity();
        }

        #endregion
        
    }
}
