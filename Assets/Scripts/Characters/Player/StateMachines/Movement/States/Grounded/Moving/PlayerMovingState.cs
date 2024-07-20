namespace GenShinImpactMovementSystem
{
    public class PlayerMovingState : PlayerGroundedState
    {
        protected PlayerMovingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine) {
        }


        #region IStates Methods

        public override void Enter() {
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.MovingParameterHash);

        }

        public override void Exit() {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.MovingParameterHash);
        }

        #endregion
        
    }
}
