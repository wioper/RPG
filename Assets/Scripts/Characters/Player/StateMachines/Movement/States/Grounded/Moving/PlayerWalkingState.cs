using UnityEngine.InputSystem;

namespace GenShinImpactMovementSystem
{
    public class PlayerWalkingState : PlayerMovingState
    {
        protected internal PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        }

        #region IState Methods

        public override void Enter() {
            
            base.Enter();
            stateMachine.ReusableData.MovementSpeedModifier = movementData.WalkData.speedModifier;
            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.WeakForce;

        }

        public override void Exit() {
            base.Exit();
            stateMachine.ReusableData.ShouldWalk = false;
        }

        #endregion
        
        #region Input Methods



        protected override void OnMovementCanceled(InputAction.CallbackContext context) {
            stateMachine.ChangeState(stateMachine.LightStoppingState);

        }

        protected override void OnWalkToggleStarted(InputAction.CallbackContext context) {
            base.OnWalkToggleStarted(context);
            stateMachine.ChangeState(stateMachine.RunningState);
        }

        #endregion

    }
}
