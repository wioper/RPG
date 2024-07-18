using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenShinImpactMovementSystem
{
    public class PlayerHardLandingState : PlayerLandingState
    {
        public PlayerHardLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
            
        }

        #region Istate Rethods

        public override void Enter() {
            stateMachine.ReusableData.MovementSpeedModifier = 0f;
            base.Enter();
            stateMachine.Player.Input.PlayerActions.Movement.Disable();
            
            ResetVelocity();
        }

        public override void Exit() {
            base.Exit();
            stateMachine.Player.Input.PlayerActions.Movement.Enable();

        }

        public override void OnAnimationExitEvent() {
            stateMachine.Player.Input.PlayerActions.Movement.Enable();

        }


        public override void OnAnimationTransitionEvent() {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }


        #endregion

        #region Resuable Methods

        protected override void AddInputActionsCallbacks() {
            base.AddInputActionsCallbacks();
            stateMachine.Player.Input.PlayerActions.Movement.started += OnMovementStarted;
        }
        
        protected override void RemoveInputActionsCallbacks() {
            base.RemoveInputActionsCallbacks();
            stateMachine.Player.Input.PlayerActions.Movement.started -= OnMovementStarted;
        }

        protected override void OnMove() {
            if (stateMachine.ReusableData.ShouldWalk) {
                return;
            }
            stateMachine.ChangeState(stateMachine.RunningState);
        }

        #endregion

        #region Input Methods

        private void OnMovementStarted(InputAction.CallbackContext context) {
            OnMove();
        }

        protected override void OnJumpStarted(InputAction.CallbackContext context) {
            
        }

        #endregion
    }
}
