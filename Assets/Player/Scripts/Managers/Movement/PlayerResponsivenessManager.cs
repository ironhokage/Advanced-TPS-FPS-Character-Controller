using Player.Managers.Helpers;
using Player.PlayerSettings.Movement;
using UnityEngine;

namespace Player.Managers.Locomotion.SubMovementManagers
{
    public class PlayerResponsivenessManager
    {
        public float ApplyDynamicResponsiveness(
        Vector3 currentInput, 
        PlayerGroundMovementHelper.SlopeData slope, 
        float ratio, 
        MovementSettings settings,
        float forwardDot,
        float alignment,
        PlayerGroundMovementHelper playerGroundMovementHelper
        )
    {
        var baseResponse = settings.BaseResponsiveness;

        var accel = settings.CurrentAccelerationSpeed;
        var decel = settings.CurrentDecelerationSpeed;

        var accelCurve = settings.accelerationResponseCurve.Evaluate(ratio);
        var decelCurve = settings.decelerationResponseCurve.Evaluate(ratio);
        
        // --- CONTROL FACTORS ---
        var pivotFactor = GetPivotResponsivenessFactor(
            currentInput.magnitude,
            alignment,
            ratio,
            settings.DirectionChangeEnterThreshold,
            settings.pivotCurve,
            settings.directionChangeSpeedCurve
        );

        var directionalFactor = playerGroundMovementHelper.GetDirectionalResponsivenessFactor(
            forwardDot, 
            settings.directionalBackwardsResponsivenessCurve,
            settings.directionalForwardsResponsivenessCurve);
        
        var controlBlend = settings.DirectionVsPivotWeight;
        var controlFactor = Mathf.Lerp(pivotFactor, directionalFactor, controlBlend);
        
        // --- BASE RESPONSE ---
        var response = Mathf.Lerp(decel, accel, controlFactor);
        
        var responseCurve = Mathf.Lerp(decelCurve, accelCurve, controlFactor);

        response *= responseCurve;
        
        // --- ENVIRONMENT FACTORS ---
        var speedFactor = Mathf.Lerp(1f, settings.HighSpeedDampening, ratio);

        var slopeFactor = ApplySlopeFactorToResponsiveness(
            settings,
            slope,
            forwardDot
        );

        var environmentFactor = speedFactor * slopeFactor;

        // --- FINAL APPLICATION ---
        response *= environmentFactor;

        var finalResponse = response * baseResponse;
        
        finalResponse = Mathf.Max(finalResponse, settings.MinResponsiveness);
        
        return finalResponse;
    }

        private static float ApplySlopeFactorToResponsiveness(MovementSettings settings, PlayerGroundMovementHelper.SlopeData slope, float forwardDot)
        {
            var forwardT = (forwardDot + 1f) / 2f;
    
            var uphill   = Mathf.Max(0f, -slope.Factor);
            var downhill = Mathf.Max(0f, slope.Factor);
            
            var uphillAxisMultiplier = Mathf.Lerp(settings.UphillBackwardResponsiveness, settings.UphillForwardResponsiveness, forwardT);
            var downhillAxisMultiplier = Mathf.Lerp(settings.DownhillBackwardResponsiveness - 1f, settings.DownhillForwardResponsiveness, forwardT);
            
            var uphillMult = uphill * (uphillAxisMultiplier - 1f);
            var downhillMult = downhill * downhillAxisMultiplier;
            
            var multiplier = 1f + uphillMult + downhillMult;
            return multiplier;
        }
        
        private static float GetPivotResponsivenessFactor(float inputMag, float alignment, float speedRatio, float directionChangeEnterThreshold, AnimationCurve responsivenessCurve, AnimationCurve speedCurve)
        {
            if (alignment >= directionChangeEnterThreshold)
                return 1f;
            
            var angleT = Mathf.InverseLerp(directionChangeEnterThreshold, -1f, alignment);
            var angleFloat = Mathf.Clamp01(responsivenessCurve.Evaluate(angleT));
            
            var speedT = Mathf.Clamp01(speedRatio);
            var speedFloat = Mathf.Clamp01(speedCurve.Evaluate(speedT));

            var computedValue = 1f - (1f - angleFloat) * speedFloat;
            var activity = Mathf.Clamp01(inputMag * speedRatio);
            
            return Mathf.Lerp(1f, computedValue, activity);

        }
        
    }
}
