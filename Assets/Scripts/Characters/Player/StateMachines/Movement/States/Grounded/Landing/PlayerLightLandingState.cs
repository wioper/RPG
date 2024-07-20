using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenShinImpactMovementSystem
{
    public class PlayerLightLandingState : PlayerLandingState
    {
        public PlayerLightLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        }

        #region IState Methods

        public override void Enter() {
            stateMachine.ReusableData.MovementSpeedModifier = 0f;
            base.Enter();


            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StationaryForce;
            
            ResetVelocity();
            
            
        }

        public override void Update() {
            base.Update();
            StartAnimation(stateMachine.Player.AnimationData.LandingParameterHash);
            if (stateMachine.ReusableData.MovementInput==Vector2.zero) {
                return;
            }
            OnMove();
        }

        public override void Exit() {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.LandingParameterHash);

        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();

            if (!IsMovingHorizontally()) {
                return;
            }
            ResetVelocity();
        }


        public override void OnAnimationTransitionEvent() {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }

        #endregion
    }
}
