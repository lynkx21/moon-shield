using UnityEngine;
using TMPro;

namespace Core
{
    [RequireComponent(typeof(OrbitalMover))]
    public class MoonMover : MonoBehaviour
    {
        private bool isHeld = false;
        private Vector3 launchDirection = Vector3.zero;
        private float launchPower = 0f;
        private float launchAngle;
        private OrbitalMover orbitalMover;
        private Vector3 startingPosition;


        // Update position
        private Vector3 frameForce;

        private void Awake()
        {
            orbitalMover = GetComponent<OrbitalMover>();
            startingPosition = transform.position;
        }


        private void FixedUpdate()
        {
            frameForce = Vector3.zero;

            if (!isHeld)
            {
                Vector3 orbitForce = orbitalMover.GetOrbitForce((int)Mathf.Sign(launchAngle));
                frameForce += orbitForce;
            }

            if (!float.IsNaN(frameForce.x) && !float.IsNaN(frameForce.y) && frameForce != Vector3.zero)
                transform.position += frameForce;
        }
    }
}