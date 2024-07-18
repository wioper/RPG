using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenShinImpactMovementSystem
{
    public class PlayerJumpingState : PlayerAirborneState
    {
        private PlayerJumpData jumpData;

        private bool shouldKeepRotating;

        private bool canStartFalling;
        
        public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
            jumpData = airborneData.JumpData;
            stateMachine.ReusableData.RotationData = jumpData.RotationData;
        }

        #region IState Methods

        public override void Enter() {
            base.Enter();
            stateMachine.ReusableData.MovementSpeedModifier = 0f;

            stateMachine.ReusableData.MovementDecelerationForce = jumpData.DecelerationForce;

            shouldKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;
            Jump();
        }

        public override void Update() {
            base.Update();
            if (!canStartFalling&&IsMovingUp(0f)) {
                canStartFalling = true;
            }
            if (!canStartFalling||GetPlayerVerticalVelocity().y>0) {
                return;
            }

            stateMachine.ChangeState(stateMachine.FallingState);
        }

        public override void Exit() {
            base.Exit();
            SetBaseRotationData();
            canStartFalling = false;

        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            if (shouldKeepRotating) {
                RotateTowardsTargetRotation();
            }

            if (IsMovingUp()) {
                DecelerateVertically();
            }
            
        }

        #endregion

        #region Resuable Methods

        protected override void ResetSprintState() {
            
            
        }

        #endregion
        
        #region Main Mathods

        private void Jump() {
            Vector3 jumpForce = stateMachine.ReusableData.CurrentJumpForce;

            Vector3 jumpDirection = stateMachine.Player.transform.forward;

            if (shouldKeepRotating) {
                UpdateTargetRotation(GetMovementInputDirection());
                jumpDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);
            }
            jumpForce.x *= jumpDirection.x;
            jumpForce.z *= jumpDirection.z;

            Vector3 capsuleColliderCenterInWordSpace =
                stateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWordSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter,out RaycastHit hit,jumpData.JumpToGroundRayDistance,
                            stateMachine.Player.LayerData.GroundLayer,QueryTriggerInteraction.Ignore)) {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);
                if (IsMovingUp()) {
                    float forceModifier = jumpData.JumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);
                    jumpForce.x *= forceModifier;
                    jumpForce.z *= forceModifier;
                }

                if (IsMovingDown()) {
                    float forceModifier = jumpData.JumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);
                    jumpForce.y *= forceModifier;
                }
                
            }
            
            ResetVelocity();

            stateMachine.Player.Rigidbody.AddForce(jumpForce,ForceMode.VelocityChange);
            
            
            

        }
        

        #endregion

        #region Inout Methods

        protected override void OnMovementCanceled(InputAction.CallbackContext context) {
            
        }

        #endregion
    }
}
