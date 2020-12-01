using System.Collections;
using UnityEngine;

namespace Core
{
    public class EnemyDash : Enemy
    {
        [SerializeField] private float waitTime = 1.5f;
        [SerializeField] private float rotationLimit = 60f;
        [SerializeField] private float distanceMultiplier = 2f;

        private Vector3 prevStartingPos = Vector3.zero;

        private bool canMove = true;

        protected override void Update()
        { 
            if (canMove)
            {
                StartCoroutine(Move());
            }           
        }

        private IEnumerator Move()
        {
            canMove = false;

            Vector3 startingPos = transform.position;
            Vector3 straightDirection = (Vector3.zero - startingPos).normalized;

            // rotate direction between -rotationLimit and rotationLimit (in degrees)
            float rotationAngle = Random.Range(-rotationLimit, rotationLimit);
            Vector3 rotatedDirection = Quaternion.AngleAxis(rotationAngle, Vector3.forward) *
                straightDirection;
            Vector3 currentDestination = startingPos + rotatedDirection * distanceMultiplier;
                        
            float distanceTotal = Vector3.Distance(currentDestination, startingPos);
            float distanceMade = 0f;

            while (distanceMade < distanceTotal)
            {
                distanceMade = Vector3.Distance(transform.position, startingPos);
                // float currentSpeed = speed * Mathf.Clamp(PercDistance(distanceMade, distanceTotal), 0.25f, 1f);
                transform.position += rotatedDirection * speed * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);
            canMove = true;
            prevStartingPos = startingPos;
            yield return null;
        }

        
        private float PercDistance(float distanceMade, float distanceTotal)
        {
            float t = distanceMade / distanceTotal;
            return 1 - Mathf.Sin(t * Mathf.PI * 0.5f);
        }
        
    }
}