using Player.Managers.Helpers;
using Player.PlayerSettings.Movement;
using UnityEngine;

namespace Player.Managers.Movement
{
    public class PlayerSpeedManager
    {
        #region Dynamic Speed

        public float ApplyDynamicSpeed(
            MovementSettings settings, 
            PlayerGroundMovementHelper.SlopeData slope, 
            float forwardDot,
            PlayerGroundMovementHelper playerGroundMovementHelper)
        {
            var baseSpeed = settings.CurrentMovementSpeed;

            var multiplier = playerGroundMovementHelper.ApplySlopeTo(settings, slope, forwardDot) * playerGroundMovementHelper.GetDirectionalResponsivenessFactor(
                forwardDot, 
                settings.directionalBackwardsSpeedCurve,
                settings.directionalForwardsSpeedCurve);
            
            baseSpeed *= multiplier;

            return Mathf.Clamp(baseSpeed, settings.MinMovementSpeed, settings.MaxMovementSpeed);
        }

        #endregion
    }
}