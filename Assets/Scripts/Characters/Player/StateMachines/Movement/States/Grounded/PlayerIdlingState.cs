
using Unity.VisualScripting;
using UnityEngine;

namespace GenShinImpactMovementSystem
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        protected internal PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
            
        }

        #region IState Methods

        public override void Enter() {
            base.Enter();
            stateMachine.ReusableData.MovementSpeedModifier = 0f;//速度调节器数值修改为0

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


        #endregion
        
    }
}
