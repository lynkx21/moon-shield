using UnityEngine;

namespace Core
{
    public class OrbitalMover: MonoBehaviour
    {
        [SerializeField] private Transform orbitCenter;

        // inner fields
        [SerializeField] private float distance;
        [SerializeField] private float omega;

        // properties
        public float Omega { get; private set; }


        private void Awake()
        {
            omega = 10f;
        }

        private void Update()
        {
            distance = Vector3.Distance(orbitCenter.position, transform.position);
            omega = OmegaFromDistance(distance);
        }

        public Vector3 GetNewOrbitPosition(int dir)
        {
            float startingAngle = Vector3.SignedAngle(Vector3.right, (transform.position - orbitCenter.position),
                Vector3.forward);
            float newFrameAngle = startingAngle * Mathf.Deg2Rad + dir * omega / distance * Time.fixedDeltaTime;

            float x = distance * Mathf.Cos(newFrameAngle);
            float y = distance * Mathf.Sin(newFrameAngle);
            return new Vector3(x, y, transform.position.z);
        }

        public Vector3 GetOrbitForce(int dir)
        {
            Vector3 newOrbitPos = GetNewOrbitPosition(dir);
            return newOrbitPos - transform.position;
        }

        private float OmegaFromDistance(float distance)
        {
            return 10f / Mathf.Pow(distance, 0.8f);
        }
    }
}