using UnityEngine;

namespace Player.Spring
{
    public class SpringVector2 : BaseSpring<Vector2>
    {
        private FloatSpring _xSpring = new();
        private FloatSpring _ySpring = new();

        public override float Damping
        {
            get => base.Damping;
            set
            {
                _xSpring.Damping = value;
                _ySpring.Damping = value;
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
                base.Stiffness = value;
            }
        }

        public override Vector2 InitialVelocity
        {
            get => new(_xSpring.InitialVelocity, _ySpring.InitialVelocity);
            
            set
            {
                _xSpring.InitialVelocity = value.x;
                _ySpring.InitialVelocity = value.y;
            }
        }

        public override Vector2 StartValue
        {
            get => new (_xSpring.StartValue, _ySpring.StartValue);
            
            set
            {
                _xSpring.StartValue = value.x;
                _ySpring.StartValue = value.y;
            }
        }
        public override Vector2 EndValue
        {
            get => new (_xSpring.EndValue, _ySpring.EndValue);
            
            set
            {
                _xSpring.EndValue = value.x;
                _ySpring.EndValue = value.y;
            }
        }

        public override Vector2 CurrentVelocity
        {
            get => new(_xSpring.CurrentVelocity, _ySpring.CurrentVelocity);
            set
            {
                _xSpring.CurrentVelocity = value.x;
                _ySpring.CurrentVelocity = value.y;
            }
        }

        public override Vector2 CurrentValue
        {
            get => new(_xSpring.CurrentValue, _ySpring.CurrentValue);
            set
            {
                _xSpring.CurrentValue = value.x;
                _ySpring.CurrentValue = value.y;
            }
        }

        public override Vector2 Evaluate(float deltaTime)
        {
            CurrentValue = new Vector2(_xSpring.Evaluate(deltaTime), _ySpring.Evaluate(deltaTime));
            CurrentVelocity = new Vector2(_xSpring.CurrentVelocity, _ySpring.CurrentVelocity);
            return CurrentValue;
        }

        public override void Reset()
        {
            _xSpring.Reset();
            _ySpring.Reset();
        }

        public override void UpdateEndValue(Vector2 value, Vector2 velocity)
        {
            _xSpring.UpdateEndValue(value.x, velocity.x);
            _ySpring.UpdateEndValue(value.y, velocity.y);
        }
    }
}
