using System.Collections;
using UnityEngine;

namespace Player.Spring.Runtime
{
    public class SpringToRotation : BaseSpringBehaviour, ISpringTo<Vector3>, ISpringTo<Quaternion>, INudgeable<Vector3>, INudgeable<Quaternion>
    {
        private SpringVector3 _spring;

        private void Awake()
        {
            _spring = new SpringVector3()
            {
                StartValue = transform.rotation.eulerAngles,
                EndValue = transform.rotation.eulerAngles,
                Damping = Damping,
                Stiffness = Stiffness
            };
        }

        public void SpringTo(Vector3 targetRotation)
        {
            SpringTo(Quaternion.Euler(targetRotation));
        }


        public void SpringTo(Quaternion targetRotation)
        {
            StopAllCoroutines();

            CheckInspectorChanges();

            StartCoroutine(DoSpringToTarget(targetRotation));
        }

        private IEnumerator DoSpringToTarget(Quaternion targetRotation)
        {
            if (Mathf.Approximately(_spring.CurrentVelocity.sqrMagnitude, 0))
            {
                _spring.Reset();
                _spring.StartValue = transform.eulerAngles;
                _spring.EndValue = targetRotation.eulerAngles;
            }
            else
            {
                _spring.UpdateEndValue(targetRotation.eulerAngles, _spring.CurrentVelocity);
            }

            while (!Mathf.Approximately(0, 1 - Quaternion.Dot(transform.rotation, targetRotation)))
            {
                transform.rotation = Quaternion.Euler(_spring.Evaluate(Time.deltaTime));

                yield return null;
            }

            _spring.Reset();
        }

        private void CheckInspectorChanges()
        {
            _spring.Damping = Damping;
            _spring.Stiffness = Stiffness;
        }

        public void Nudge(Vector3 amount)
        {
            CheckInspectorChanges();
            if (Mathf.Approximately(_spring.CurrentVelocity.sqrMagnitude, 0))
            {
                StartCoroutine(HandleNudge(amount));
            }
            else
            {
                _spring.UpdateEndValue(_spring.EndValue, _spring.CurrentVelocity + amount);
            }
        }

        private IEnumerator HandleNudge(Vector3 amount)
        {
            _spring.Reset();
            
            _spring.StartValue = transform.rotation.eulerAngles;
            _spring.EndValue = transform.rotation.eulerAngles;
            _spring.InitialVelocity = amount;
            
            var targetRotation = transform.rotation;
            transform.rotation = Quaternion.Euler(_spring.Evaluate(Time.deltaTime));

            while (!Mathf.Approximately(
                0,
                1 - Quaternion.Dot(targetRotation, transform.rotation)
            ))
            {
                transform.rotation = Quaternion.Euler(_spring.Evaluate(Time.deltaTime));

                yield return null;
            }

            _spring.Reset();
        }

        public void Nudge(Quaternion amount)
        {
            Nudge(amount.eulerAngles);
        }
    }
}