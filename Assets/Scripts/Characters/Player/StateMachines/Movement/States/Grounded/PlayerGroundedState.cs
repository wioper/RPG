using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenShinImpactMovementSystem
{
    public class PlayerGroundedState : PlayerMovementState
    {

        private SlopeData slopeData;
        protected PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
            slopeData = stateMachine.Player.ColliderUtility.SlopeData;
        }


        #region IState Methods

        public override void Enter() {
            base.Enter();
            UpdateShouldSprintState();

        }

        

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();

            Float();
        }

        #region Main Methods

        private void UpdateShouldSprintState() {
            if (stateMachine.ReusableData.ShouldSprint) {
                return;
            }

            if (stateMachine.ReusableData.MovementInput!=Vector2.zero) {
                return;
            }

            stateMachine.ReusableData.ShouldSprint = false;
        }
        private void Float() {
            Vector3 capsuleColliderCenterInWordSpace =
                stateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWordSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter,out RaycastHit hit,slopeData.FloatRayDistance,stateMachine.Player.LayerData.GroundLayer,QueryTriggerInteraction.Ignore)) {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);
                if (slopeSpeedModifier==0f) {
                    return;
                }
                
                float distanceToFloatingPoint =
                    stateMachine.Player.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y *
                    stateMachine.Player.transform.localScale.y - hit.distance;

                if (distanceToFloatingPoint==0f) {
                    return;
                }

                float amountToLift = distanceToFloatingPoint * slopeData.StepReachForce-GetPlayerVerticalVelocity().y;

                Vector3 liftForce = new Vector3(0f, amountToLift, 0f);
                stateMachine.Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);

            }
            
        }

        private float SetSlopeSpeedModifierOnAngle(float angle) {
            float slopeSpeedModifier = movementData.SlopeSpeedAngles.Evaluate(angle);
            stateMachine.ReusableData.MovementSlopesSpeedModifier = slopeSpeedModifier;
            
            return slopeSpeedModifier;
        }
        
        private bool IsThereGroundUnderneath() {
            BoxCollider groundCheckGround = stateMachine.Player.ColliderUtility.TriggerColliderData.GroundCheckCollider;
            Vector3 groundColliderCenterInWordSpace = groundCheckGround.bounds.center;
            Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWordSpace,
                groundCheckGround.bounds.extents, groundCheckGround.transform.rotation,
                stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore);
            return overlappedGroundColliders.Length > 0;
        }

        #endregion
        
        

        #endregion
        

        #region Reusable Methods

        protected override void AddInputActionsCallbacks() {
            base.AddInputActionsCallbacks();

            stateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
            stateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;
            stateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;
        }




        protected override void RemoveInputActionsCallbacks() {
            base.RemoveInputActionsCallbacks();
            stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
            stateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;
            stateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;


        }
        
        protected virtual void OnMove() {
            if (stateMachine.ReusableData.ShouldSprint) {
                stateMachine.ChangeState(stateMachine.SprintingState);
                return;
            }

            if (stateMachine.ReusableData.ShouldWalk) {
                stateMachine.ChangeState(stateMachine.WalkingState);
                return;
            }

            stateMachine.ChangeState(stateMachine.RunningState);
            
        }

        protected override void OnContactWithGroundExited() {
            base.OnContactWithGroundExited();

            if (IsThereGroundUnderneath()) {
                return;
            }
            Vector3 capsuleColliderCenterInWordSpace =
                stateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleBottom =
                new Ray(
                    capsuleColliderCenterInWordSpace - stateMachine.Player.ColliderUtility.CapsuleColliderData
                        .ColliderVerticalExtents, Vector3.down);
            if (!Physics.Raycast(downwardsRayFromCapsuleBottom,out _,movementData.GroundToFallRayDistance,stateMachine.Player.LayerData.GroundLayer,QueryTriggerInteraction.Ignore)) {
                
                OnFall();
            }
        }

        

        protected virtual void OnFall() {
            stateMachine.ChangeState(stateMachine.FallingState);

        }

        #endregion
        
        
        
        
        #region Input Methods
        protected virtual void OnMovementCanceled(InputAction.CallbackContext context) {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
        protected virtual void OnDashStarted(InputAction.CallbackContext context) {
            stateMachine.ChangeState(stateMachine.DashingState);
        }
        protected virtual void OnJumpStarted(InputAction.CallbackContext context) {
            stateMachine.ChangeState(stateMachine.JumpingState);
        }
        
     


        #endregion
    }
}