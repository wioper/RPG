using UnityEngine.InputSystem;

namespace GenShinImpactMovementSystem
{
    public class PlayerWalkingState : PlayerMovingState
    {
        private PlayerWalkData walkData;
        protected internal PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
            walkData = movementData.WalkData;
            
        }

        #region IState Methods

        public override void Enter() {
            
            stateMachine.ReusableData.MovementSpeedModifier = walkData.speedModifier;
            stateMachine.ReusableData.BackwardsCameraRecenteringData = walkData.BackwardsCameraRecenteringData;
            
            base.Enter();
            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.WeakForce;

        }

        public override void Exit() {
            base.Exit();
            stateMachine.ReusableData.ShouldWalk = false;
            SetBaseCameraRecenteringData();
        }

        #endregion
        
        #region Input Methods



        protected override void OnMovementCanceled(InputAction.CallbackContext context) {
            stateMachine.ChangeState(stateMachine.LightStoppingState);
            base.OnMovementCanceled(context);
        }

        protected override void OnWalkToggleStarted(InputAction.CallbackContext context) {
            base.OnWalkToggleStarted(context);
            stateMachine.ChangeState(stateMachine.RunningState);
        }

        #endregion

    }
}
