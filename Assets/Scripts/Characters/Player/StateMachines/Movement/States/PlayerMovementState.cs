using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenShinImpactMovementSystem
{
    public class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine stateMachine;
        

        protected PlayerGroundedData movementData;

        protected PlayerAirborneData airborneData;
        


        protected PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine) {
            stateMachine = playerMovementStateMachine;
            movementData = stateMachine.Player.Data.GroundedData;
            airborneData = stateMachine.Player.Data.AirborneData;
            InitializeData();
        }

        private void InitializeData() {
            SetBaseRotationData();

        }


        #region IState Methods
        public virtual void Enter() {
            Debug.Log("State: "+GetType().Name);

            AddInputActionsCallbacks();
        }



        public virtual void Exit() {
            RemoveInputActionsCallbacks();
        }

        

        public virtual void HandleInput() {
            ReadMovementInput();
        }

        public virtual void Update() {
            
        }

        public virtual void PhysicsUpdate() {
            Move();
        }

        public virtual void OnAnimationEnterEvent() {
            
        }

        public virtual void OnAnimationExitEvent() {
            
        }

        public virtual void OnAnimationTransitionEvent() {
            
        }

        public virtual void OnTriggerEnter(Collider collider) {
            if (stateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer)) {
                OnContactWithGround(collider);
                return;
            }
        }

        public void OnTriggerExit(Collider collider) {
            if (stateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer)) {
                OnContactWithGroundExited();
                return;
            }
        }



        #endregion


        #region Main Methods

        private void ReadMovementInput() {
            //从New Input System中读取Vector2数据 
            stateMachine.ReusableData.MovementInput = stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
        }
        private void Move() {
            if (stateMachine.ReusableData.MovementInput==Vector2.zero||stateMachine.ReusableData.MovementSpeedModifier==0f) {
                return;
            }
            Vector3 movementDirection = GetMovementInputDirection();

            float targetRotationYAngle = Rotate(movementDirection);

            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

            float movementSpeed = GetMovementSpeed();

            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();
            stateMachine.Player.Rigidbody.AddForce(targetRotationDirection*movementSpeed -currentPlayerHorizontalVelocity,ForceMode.VelocityChange);

        }


        private float Rotate(Vector3 direction) {

            float directionAngle = UpdateTargetRotation(direction);

            RotateTowardsTargetRotation();
            
            return directionAngle;
            
        }



        //获取移动的度数
        private float GetDirectionAngle(Vector3 direction) {
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            if (directionAngle<0f) {
                directionAngle += 360f;
            }
            
            return directionAngle;
        }
        
        private float AddCameraRotationToAngle(float angle) {
            angle += stateMachine.Player.MainCameraTransform.eulerAngles.y;

            if (angle>360f) {
                angle -= 360f;
            }


            return angle;
        }

        
        private void UpdateTargetRotationData(float targetAngle) {
            stateMachine.ReusableData.CurrentTargetRotation.y = targetAngle;

            stateMachine.ReusableData.DampedTargetRotationPassedTime.y = 0f;
        }


        #endregion

        #region Reusable Methods

        
                

        protected virtual void SetBaseRotationData() {
            stateMachine.ReusableData.RotationData = movementData.BaseRotationData;
            stateMachine.ReusableData.TimeToTeachTargetRotation =stateMachine.ReusableData.RotationData.TargetRotationTime;
        }
        //获取移动的是x和z，不考虑y的变化
        protected Vector3 GetMovementInputDirection() {
            return new Vector3(stateMachine.ReusableData.MovementInput.x, 0f, stateMachine.ReusableData.MovementInput.y);
        }
        
        
        protected float GetMovementSpeed() {
            return movementData.BaseSpeed * stateMachine.ReusableData.MovementSpeedModifier*stateMachine.ReusableData.MovementSlopesSpeedModifier;
        }
        protected Vector3 GetPlayerHorizontalVelocity() {
            Vector3 playerHorizontalVelocity = stateMachine.Player.Rigidbody.velocity;
            playerHorizontalVelocity.y = 0f;
            return playerHorizontalVelocity;

        }

        protected Vector3 GetPlayerVerticalVelocity() {
            return new Vector3(0f, stateMachine.Player.Rigidbody.velocity.y, 0f);
        }
        
        
        protected void RotateTowardsTargetRotation() {
            float currentYAngle = stateMachine.Player.Rigidbody.rotation.eulerAngles.y;

            if (currentYAngle==stateMachine.ReusableData.CurrentTargetRotation.y) {
                return;
            }

            float smoothYAngle = Mathf.SmoothDampAngle(currentYAngle, stateMachine.ReusableData.CurrentTargetRotation.y,
                ref stateMachine.ReusableData.DampedTargetRotationCurrentVelocity.y,
                stateMachine.ReusableData.TimeToTeachTargetRotation.y - stateMachine.ReusableData.DampedTargetRotationPassedTime.y);

            stateMachine.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;
            
            Quaternion targetRotation=Quaternion.Euler(0f,smoothYAngle,0f);

            stateMachine.Player.Rigidbody.MoveRotation(targetRotation);

        }
        
        
        protected float UpdateTargetRotation(Vector3 direction,bool shouldConsiderCameraRotation=true) {
            float directionAngle = GetDirectionAngle(direction);
            if (shouldConsiderCameraRotation) {
                directionAngle = AddCameraRotationToAngle(directionAngle);
            }

            if (directionAngle!=stateMachine.ReusableData.CurrentTargetRotation.y) {
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }
        
        protected Vector3 GetTargetRotationDirection(float targetAngle) {
            return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        protected void ResetVelocity() {
            stateMachine.Player.Rigidbody.velocity = Vector3.zero;
        }
        
        protected virtual void ResetVerticalVelocity() {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
            stateMachine.Player.Rigidbody.velocity = playerHorizontalVelocity;
        }
        
        protected virtual void AddInputActionsCallbacks() {
            stateMachine.Player.Input.PlayerActions.WalkToggle.started += OnWalkToggleStarted;
        }



        protected virtual void RemoveInputActionsCallbacks() {
            stateMachine.Player.Input.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;
        }

        protected void DecelerateHorizontally() {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
            stateMachine.Player.Rigidbody.AddForce(
                -playerHorizontalVelocity * stateMachine.ReusableData.MovementDecelerationForce,
                ForceMode.Acceleration);
        }
        
        protected void DecelerateVertically() {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();
            stateMachine.Player.Rigidbody.AddForce(
                -playerVerticalVelocity * stateMachine.ReusableData.MovementDecelerationForce,
                ForceMode.Acceleration);
        }
        
        

        protected bool IsMovingHorizontally(float minimumMagnitude=0.1f) {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

            Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.z);
            return playerHorizontalMovement.magnitude > minimumMagnitude;
        }

        protected bool IsMovingUp(float minimumVelocity=0.1f) {
            return GetPlayerVerticalVelocity().y > minimumVelocity;
        }
        
        protected bool IsMovingDown(float minimumVelocity=0.1f) {
            return GetPlayerVerticalVelocity().y <-minimumVelocity;
        }
        
        protected virtual void OnContactWithGround(Collider collider) {
            
        }
        
        protected virtual void OnContactWithGroundExited() {
            
        }

        #endregion

        #region Input Methods

        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context) {
            stateMachine.ReusableData.ShouldWalk = !stateMachine.ReusableData.ShouldWalk;
        }

        #endregion

    }
}