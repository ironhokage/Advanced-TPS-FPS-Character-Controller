using System.Collections;
using UnityEngine;

namespace Player.Spring.Runtime
{
    public class SpringToTarget2D : BaseSpringBehaviour, ISpringTo<Vector2>, INudgeable<Vector2>
    {
        private SpringVector2 _spring;

        private void Awake()
        {
            _spring = new SpringVector2()
            {
                StartValue = transform.position,
                EndValue = transform.position,
                Damping = Damping,
                Stiffness = Stiffness
            };
        }

        private IEnumerator DoSpringToTarget(Vector2 targetPosition)
        {
            if (Mathf.Approximately(_spring.CurrentVelocity.sqrMagnitude, 0))
            {
                _spring.Reset();
                _spring.StartValue = transform.position;
                _spring.EndValue = targetPosition;
            }
            else
            {
                _spring.UpdateEndValue(targetPosition, _spring.CurrentVelocity);
            }

            while (!Mathf.Approximately(Vector2.SqrMagnitude(
                    new Vector2(transform.position.x,
                        transform.position.y
                        ) - targetPosition), 0))
            {
                transform.position = _spring.Evaluate(Time.deltaTime);

                yield return null;
            }

            _spring.Reset();
        }

        public void SpringTo(Vector2 targetPosition)
        {
            StopAllCoroutines();

            CheckInspectorChanges();

            StartCoroutine(DoSpringToTarget(targetPosition));
        }

        private void CheckInspectorChanges()
        {
            _spring.Damping = Damping;
            _spring.Stiffness = Stiffness;
        }

        public void Nudge(Vector2 amount)
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

        private IEnumerator HandleNudge(Vector2 Amount)
        {
            _spring.Reset();
            _spring.StartValue = transform.position;
            _spring.EndValue = transform.position;
            _spring.InitialVelocity = Amount;
            var targetPosition = transform.position;
            transform.position = _spring.Evaluate(Time.deltaTime);

            while (!Mathf.Approximately(
                0,
                Vector2.SqrMagnitude(targetPosition - transform.position)
            ))
            {
                transform.position = _spring.Evaluate(Time.deltaTime);

                yield return null;
            }

            _spring.Reset();
        }
    }
}