using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenShinImpactMovementSystem
{
    public class PlayerLandingState : PlayerGroundedState
    {
        public PlayerLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        }

        #region Input Methods

        protected override void OnMovementCanceled(InputAction.CallbackContext context) {
            
        }

        #endregion
        
    }
}
