using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class EnemySin : Enemy
    {
        [SerializeField] private float amplitude = 10f;
        [SerializeField] private float frequency = 0.75f;
        private Vector3 startingPosition;

        protected override void Start()
        {
            base.Start();
            startingPosition = transform.position;
        }

        protected override void Update()
        {
            Vector3 direction = (Vector3.zero - transform.position).normalized;
            Vector3 rotatedDir = new Vector3(direction.y, -direction.x, direction.z);
            Vector3 fluctuation = rotatedDir * Mathf.Lerp(amplitude, 0f, PercDistance()) *
                Mathf.Sin(frequency * Time.time);
            // direction += Vector3.up * period * Mathf.Sin(period * Time.time);
            transform.position += (direction + fluctuation) * speed * Time.deltaTime;
        }

        private float PercDistance()
        {
            return 1f - (Vector3.zero - transform.position).magnitude /
                (Vector3.zero - startingPosition).magnitude;
        }

        public override Vector3 GetDirection()
        {
            // Debug.Log("[EnemyRandom] computing sin direction!");
            Vector3 straightDirection = (destination - transform.position).normalized;
            float currentSinValue = Mathf.Sin(Time.time) * 2f;
            float futureSinValue = Mathf.Sin(Time.time + Time.deltaTime) * 2f;
            Vector3 rotatedDirection = new Vector3(straightDirection.y, -straightDirection.x,
                straightDirection.z);
            Vector3 sinDirection = (futureSinValue - currentSinValue) * Vector3.up;
            // return straightDirection + sinDirection;
            return sinDirection;
        }
    }
}