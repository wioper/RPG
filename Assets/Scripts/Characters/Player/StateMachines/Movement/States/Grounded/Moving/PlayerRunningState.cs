using UnityEngine;
using UnityEngine.InputSystem;

namespace GenShinImpactMovementSystem
{
    public class PlayerRunningState : PlayerMovingState
    {

        private PlayerSprintData sprintData;

        private float startTime;
        
        protected internal PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
            startTime = Time.time;
            sprintData = movementData.SprintData;
            
        }

        #region IStates

        public override void Enter() {
            stateMachine.ReusableData.MovementSpeedModifier = movementData.RunData.speedModifier;
            base.Enter();
            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.MediumForce;

        }

        public override void Update() {
            base.Update();
            if (!stateMachine.ReusableData.ShouldWalk) {
                return;
            }

            if (Time.time<startTime+sprintData.RunToWalkTime) {
                return;
            }

            StopRunning();
        }

 

        #endregion

        #region Main Mathods
        

        private void StopRunning() {
            if (stateMachine.ReusableData.MovementInput!=Vector2.zero) {
                stateMachine.ChangeState(stateMachine.IdlingState);
                return;
            }
            stateMachine.ChangeState(stateMachine.WalkingState);

        }

        #endregion
        
        
        #region Input Methods

        protected override void OnMovementCanceled(InputAction.CallbackContext context) {
            stateMachine.ChangeState(stateMachine.MediumStoppingState);
            base.OnMovementCanceled(context);

        }
        
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context) {
            base.OnWalkToggleStarted(context);
            stateMachine.ChangeState(stateMachine.WalkingState);
        }

        #endregion

    }
}
