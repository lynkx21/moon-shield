using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class EnemyRandom : Enemy
    {
        // private Vector3 destination = Vector3.zero;
        protected override void Update()
        {
            Vector3 direction = this.GetDirection();
            transform.position += direction * speed * Time.deltaTime;
        }

        public override Vector3 GetDirection()
        {
            Vector3 straightDirection = (destination - transform.position).normalized;
            float directionAngle = -Vector3.SignedAngle(straightDirection, transform.up, Vector3.forward);

            Vector3 randDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), transform.position.z);

            return straightDirection + randDirection;
        }
    }
}