using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenShinImpactMovementSystem
{
    public class PlayerAirborneState :PlayerMovementState
    {
        protected PlayerAirborneState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
            
        }

        #region IState Methods

        public override void Enter() {
            base.Enter();
            ResetSprintState();
        }

        #endregion

        #region Resuable Methods
        protected override void OnContactWithGround(Collider collider) {
            stateMachine.ChangeState(stateMachine.LightLandingState);
        }

        protected virtual void ResetSprintState() {
            stateMachine.ReusableData.ShouldSprint = false;
        }

        #endregion
        
    }
}
