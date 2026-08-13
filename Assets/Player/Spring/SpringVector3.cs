using UnityEngine;

namespace Player.Spring
{
    public class SpringVector3 : BaseSpring<Vector3>
    {
        private FloatSpring _xSpring = new();
        private FloatSpring _ySpring = new();
        private FloatSpring _zSpring = new();

        public override float Damping
        {
            get => base.Damping;
            set
            {
                _xSpring.Damping = value;
                _ySpring.Damping = value;
                _zSpring.Damping = value;
                base.Damping = value;
            }
        }

        public override float Stiffness
        {
            get => base.Stiffness;
            set
            {
                _xSpring.Stiffness = value;
                _ySpring.Stiffness = value;
                _zSpring.Stiffness = value;
                base.Stiffness = value;
            }
        }

        public override Vector3 StartValue
        {
            get => new(
                    _xSpring.StartValue,
                    _ySpring.StartValue,
                    _zSpring.StartValue
                );
            set
            {
                _xSpring.StartValue = value.x;
                _ySpring.StartValue = value.y;
                _zSpring.StartValue = value.z;
            }
        }
        public override Vector3 EndValue
        {
            get => new(
                    _xSpring.EndValue, 
                    _ySpring.EndValue, 
                    _zSpring.EndValue
                );
            set
            {
                _xSpring.EndValue = value.x;
                _ySpring.EndValue = value.y;
                _zSpring.EndValue = value.z;
            }
        }

        public override Vector3 InitialVelocity
        {
            get => new(
                    _xSpring.InitialVelocity, 
                    _ySpring.InitialVelocity, 
                    _zSpring.InitialVelocity
                );
            set
            {
                _xSpring.InitialVelocity = value.x;
                _ySpring.InitialVelocity = value.y;
                _zSpring.InitialVelocity = value.z;
            }
        }

        public override Vector3 CurrentVelocity
        {
            get => new(
                    _xSpring.CurrentVelocity, 
                    _ySpring.CurrentVelocity, 
                    _zSpring.CurrentVelocity
                );
            set
            {
                _xSpring.CurrentVelocity = value.x;
                _ySpring.CurrentVelocity = value.y;
                _zSpring.CurrentVelocity = value.z;
            }
        }

        public override Vector3 CurrentValue
        {
            get => new(
                    _xSpring.CurrentValue, 
                    _ySpring.CurrentValue, 
                    _zSpring.CurrentValue
                );
            set
            {
                _xSpring.CurrentValue = value.x;
                _ySpring.CurrentValue = value.y;
                _zSpring.CurrentValue = value.z;
            }
        }

        public override Vector3 Evaluate(float deltaTime)
        {
            CurrentValue = new Vector3(
                _xSpring.Evaluate(deltaTime),
                _ySpring.Evaluate(deltaTime),
                _zSpring.Evaluate(deltaTime)
            );
            CurrentVelocity = new Vector3(
                _xSpring.CurrentVelocity, 
                _ySpring.CurrentVelocity, 
                _zSpring.CurrentVelocity
            );
            return CurrentValue;
        }
        

        public override void Reset()
        {
            _xSpring.Reset();
            _ySpring.Reset();
            _zSpring.Reset();
        }

        public override void UpdateEndValue(Vector3 value, Vector3 velocity)
        {
            _xSpring.UpdateEndValue(value.x, velocity.x);
            _ySpring.UpdateEndValue(value.y, velocity.y);
            _zSpring.UpdateEndValue(value.z, velocity.z);
        }
    }
}
