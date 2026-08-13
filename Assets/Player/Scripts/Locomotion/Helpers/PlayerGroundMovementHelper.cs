using Player.Interfaces.Movement.Context;
using Player.PlayerSettings.Movement;
using UnityEngine;
namespace Player.Managers.Helpers
{
    public class PlayerGroundMovementHelper
    {
        public struct SlopeData
        {
            public float SlopeDot;        // [-1, 1]  (+downhill, -uphill)
            public float Steepness;       // [0, 1]
            public float Factor;          // [-1, 1]  final combined metric
        }
        private float _previousFactor;
        
        #region Interface Implementation
        
        private static Vector3 ForwardBody => PlayerContext.Orientation.ForwardBody;
        private static Vector3 RightBody => PlayerContext.Orientation.RightBody;
        private static Vector3 CharacterUp => PlayerContext.Orientation.CharacterUp;
        private static Vector3 GroundNormal => PlayerContext.Movement.GroundNormal;
        private static Vector3 DownhillDir => PlayerContext.Movement.DownhillDir;
        private static float ClampedDeltaTime => PlayerContext.State.ClampedDeltaTime;
        #endregion
        
        public SlopeData CalculateSlope(
            float maxSlopeAngle, 
            MovementSettings settings,
            Vector3 velocity
            )
        {
            var slopeAngle = Vector3.Angle(CharacterUp, GroundNormal);
            
            if (slopeAngle < 1.0f)
            {
                return new SlopeData
                {
                    SlopeDot = 0f,
                    Steepness = 0f,
                    Factor = 0f 
                };
            }
            var safeMaxAngle = Mathf.Max(0.01f, maxSlopeAngle);
            var steepness = Mathf.Clamp01(slopeAngle / safeMaxAngle);

            var moveDir = velocity.sqrMagnitude > 0.001f ? velocity.normalized : ForwardBody;
            if (moveDir.sqrMagnitude < 0.001f)
            {
                return new SlopeData { SlopeDot = 0f, Steepness = steepness, Factor = 0f };
            }
            
            var forwardMove = Vector3.Dot(moveDir, ForwardBody);
            var rightMove = Vector3.Dot(moveDir, RightBody);
            
            var forwardDownhill = Vector3.Dot(ForwardBody, DownhillDir);
            var rightDownhill = Vector3.Dot(RightBody, DownhillDir);
            
            var forwardAlignment = Mathf.Abs(forwardDownhill);
            var forwardMultiplier = forwardAlignment > 0.5f ? 1f : 0f;  
            
            var forwardMomentum = forwardMove * forwardDownhill * forwardMultiplier;
            var rightMomentum = rightMove * rightDownhill;
            
            rightMomentum = Mathf.Max(steepness, rightMomentum);
            
            var signMult = forwardDownhill < 0f ? -1f : 1f;

            rightMomentum = signMult * Mathf.Abs(rightMomentum);
            
            var slopeDot = forwardMomentum + rightMomentum;

            slopeDot = Mathf.Clamp(slopeDot, -1f, 1f);
            
            if (Mathf.Abs(slopeDot) < 0.01f)
            {
                return new SlopeData { SlopeDot = 0f, Steepness = steepness, Factor = 0f };
            }
            
            var finalFactor = Mathf.Clamp(slopeDot * steepness, -1f, 1f);
            
            var total = Mathf.Abs(forwardMove) + Mathf.Abs(rightMove);
            var forwardWeight = total > 0.001f ? Mathf.Abs(forwardMove) / total : Mathf.Clamp01(forwardAlignment / 0.5f);
            
            var target = Mathf.Lerp(rightMomentum, finalFactor, forwardWeight);

            var t = 1f - Mathf.Exp(-settings.TransitionValue * ClampedDeltaTime);
            _previousFactor = Mathf.Lerp(_previousFactor, target, t);
            
            return new SlopeData
            {
                SlopeDot = slopeDot,
                Steepness = steepness,
                Factor = _previousFactor
            };
        }
        
        public float GetDirectionalResponsivenessFactor(
            float forwardDot,
            AnimationCurve backwardsCurve,
            AnimationCurve forwardsCurve,
            float deadZone = 0.15f)
        {
            var absForwardDot = Mathf.Abs(forwardDot);
            
            if (forwardDot > deadZone)
            {
                return forwardsCurve.Evaluate(forwardDot);
            }
            if (forwardDot < -deadZone)
            {
                return backwardsCurve.Evaluate(absForwardDot);
            }
            var t = Mathf.InverseLerp(-deadZone, deadZone, forwardDot);
            
            var forwardValue = forwardsCurve.Evaluate(forwardDot);
            var backwardValue = backwardsCurve.Evaluate(absForwardDot);
            
            return Mathf.Lerp(backwardValue, forwardValue, t);
        }
        
        public float ApplySlopeTo(MovementSettings settings, SlopeData slope, float forwardDot)
        {
            var forwardT = (forwardDot + 1f) / 2f;
    
            var uphill   = Mathf.Max(0f, -slope.Factor);
            var downhill = Mathf.Max(0f, slope.Factor);
            
            var uphillAxisMultiplier = Mathf.Lerp(settings.UphillBackwardSpeedFactor, settings.UphillForwardSpeedFactor, forwardT);
            var downhillAxisMultiplier = Mathf.Lerp(settings.DownhillBackwardSpeedFactor - 1f, settings.DownhillForwardSpeedFactor, forwardT);
            
            var uphillMult = uphill * (uphillAxisMultiplier - 1f);
            var downhillMult = downhill * downhillAxisMultiplier;
            
            var multiplier = 1f + uphillMult + downhillMult;
            return multiplier;
        }
    }
}