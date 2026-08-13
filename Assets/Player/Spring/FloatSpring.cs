using UnityEngine;

/* Code obtained originally from: https://github.com/thammin/unity-spring. 
 * Modified by Chris from LlamAcademy
MIT License

Copyright (c) 2019 Paul Young
Copyright (c) 2022 LlamAcademy

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
/// <summary>
/// Closed-form solution for the ODE of damped harmonic oscillator.
/// https://en.wikipedia.org/wiki/Harmonic_oscillator#Damped_harmonic_oscillator
///
/// Proof and derived from http://www.ryanjuckett.com/programming/damped-springs/
/// </summary
/// 
namespace Player.Spring
{
    public class FloatSpring : BaseSpring<float>
    {
        /// <summary>
        /// Closed-form solution for the ODE of damped harmonic oscillator.
        /// https://en.wikipedia.org/wiki/Harmonic_oscillator#Damped_harmonic_oscillator
        ///
        /// Proof and derived from http://www.ryanjuckett.com/programming/damped-springs/
        /// </summary>
        private float _springTime;

        public override void Reset()
        {
            _springTime = 0f;
            CurrentValue = 0f;
            CurrentVelocity = 0f;
            InitialVelocity = 0f;
        }

        public override void UpdateEndValue(float value, float velocity)
        {
            StartValue = CurrentValue;
            EndValue = value;
            InitialVelocity = velocity;
            _springTime = 0f;
        }

        public override float Evaluate(float deltaTime)
        {
            _springTime += deltaTime;

            var c = Damping;
            var m = Mass;
            var k = Stiffness;
            var v0 = -InitialVelocity;
            var t = _springTime;

            var zeta = c / (2 * Mathf.Sqrt(k * m)); // damping ratio
            var omega0 = Mathf.Sqrt(k / m); // undamped angular frequency of the oscillator (rad/s)
            var x0 = EndValue - StartValue;

            var omegaZeta = omega0 * zeta;
            float x;
            float v;

            switch (zeta)
            {
                // Under damped
                case < 1:
                {
                    var omega1 = omega0 * Mathf.Sqrt(1.0f - zeta * zeta); // exponential decay
                    var e = Mathf.Exp(-omegaZeta * t);
                    var c2 = (v0 + omegaZeta * x0) / omega1;
                    var cos = Mathf.Cos(omega1 * t);
                    var sin = Mathf.Sin(omega1 * t);
                    x = e * (x0 * cos + c2 * sin);
                    v = -e * ((x0 * omegaZeta - c2 * omega1) * cos + (x0 * omega1 + c2 * omegaZeta) * sin);
                    break;
                }
                // Over damped
                case > 1:
                {
                    var omega2 = omega0 * Mathf.Sqrt(zeta * zeta - 1.0f); // frequency of damped oscillation
                    var z1 = -omegaZeta - omega2;
                    var z2 = -omegaZeta + omega2;
                    var e1 = Mathf.Exp(z1 * t);
                    var e2 = Mathf.Exp(z2 * t);
                    var c1 = (v0 - x0 * z2) / (-2 * omega2);
                    var c2 = x0 - c1;
                    x = c1 * e1 + c2 * e2;
                    v = c1 * z1 * e1 + c2 * z2 * e2;
                    break;
                }
                // Critically damped
                default:
                {
                    var e = Mathf.Exp(-omega0 * t);
                    x = e * (x0 + (v0 + omega0 * x0) * t);
                    v = e * (v0 * (1 - t * omega0) + t * x0 * (omega0 * omega0));
                    break;
                }
            }

            CurrentValue = EndValue - x;
            CurrentVelocity = v;
            
            return CurrentValue;
        }
    }
}
