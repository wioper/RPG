using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenShinImpactMovementSystem
{
    public class PlayerDashingState : PlayerGroundedState
    {

        private PlayerDashData dashData;

        private float startTime;

        private int consecutiveDashesUsed;

        private bool shouleKeepRotating;
        public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
            dashData = movementData.DashData;
        }

        #region Istates Methods

        public override void Enter() {
            base.Enter();
            stateMachine.ReusableData.MovementSpeedModifier = dashData.SpeedModifier;
            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;


            stateMachine.ReusableData.RotationData = dashData.RotationData;

            AddForceOnTransitionFromStationaryState();

            shouleKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;

            UpdateConsecutiveDashes();

            startTime = Time.time;
        }

        public override void Exit() {
            base.Exit();
            SetBaseRotationData();
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            if (!shouleKeepRotating) {
                return;
            }
            RotateTowardsTargetRotation();
        }

        public override void OnAnimationTransitionEvent() {
            if (stateMachine.ReusableData.MovementInput == Vector2.zero) {
                stateMachine.ChangeState(stateMachine.HardStoppingState);
                return;
            }
            
            stateMachine.ChangeState(stateMachine.SprintingState);

            

        }

        #endregion

        

        #region Main Methods

        private void AddForceOnTransitionFromStationaryState() {
            if (stateMachine.ReusableData.MovementInput!=Vector2.zero) {
                return;
            }

            Vector3 characterRotationDirection = stateMachine.Player.transform.forward;

            characterRotationDirection.y = 0f;

            UpdateTargetRotation(characterRotationDirection, false);

            stateMachine.Player.Rigidbody.velocity = characterRotationDirection * GetMovementSpeed();
        }
        
        private void UpdateConsecutiveDashes() {
            if (!IsConsecutive()) {
                consecutiveDashesUsed = 0;
            }

            ++consecutiveDashesUsed;

            if (consecutiveDashesUsed==dashData.ConsecutiveDashesLimitAmount) {
                consecutiveDashesUsed = 0;

                stateMachine.Player.Input.DisableActionFor(stateMachine.Player.Input.PlayerActions.Dash,
                    dashData.DashLimitReachedCooldown);
            }

        }

        private bool IsConsecutive() {
            return Time.time < startTime + dashData.TimeToBeConsiderConsecutive;
        }

        #endregion

        #region Resuable Methods

        protected override void AddInputActionsCallbacks() {
            base.AddInputActionsCallbacks();
            
            stateMachine.Player.Input.PlayerActions.Movement.performed+=OnMovementPerformed;
        }


        protected override void RemoveInputActionsCallbacks() {
            base.RemoveInputActionsCallbacks();
            stateMachine.Player.Input.PlayerActions.Movement.performed-=OnMovementPerformed;

        }



        #endregion

        #region Input Methods

        protected override void OnMovementCanceled(InputAction.CallbackContext context) {
            
        }
        
        private void OnMovementPerformed(InputAction.CallbackContext context) {
            shouleKeepRotating = true;
        }


        protected override void OnDashStarted(InputAction.CallbackContext context) {
            
        }

        #endregion
        
    }
}
