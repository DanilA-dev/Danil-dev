using D_Dev.PolymorphicValueSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace D_Dev.PlayerStateController
{
    public class PlayerRunState : BasePlayerState
    {
        #region Fields

        [Title("Movement Handler")]
        [SerializeField] private PlayerMovementHandler _movementHandler;
        [Title("Run Settings")]
        [SerializeReference] private PolymorphicValue<float> _runAcceleration;
        [SerializeReference] private PolymorphicValue<float> _runMaxSpeed;
        [SerializeReference] private PolymorphicValue<float> _rotationSpeed;

        #endregion
        
        #region Properties

        public override PlayerStateName StateName => PlayerStateName.Run;

        #endregion

        #region IState

        public override void OnUpdate()
        {
            _movementHandler.FaceMovementDirection(_rotationSpeed.Value);
        }

        public override void OnFixedUpdate()
        {
            _movementHandler.ApplyMovement(_runAcceleration.Value, _runMaxSpeed.Value);
        }

        #endregion
    }
}
