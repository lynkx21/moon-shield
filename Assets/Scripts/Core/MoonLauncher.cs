using UnityEngine;
using TMPro;

namespace Core
{
    [RequireComponent(typeof(OrbitalMover))]
    public class MoonLauncher : MonoBehaviour
    {
        private bool isHeld = false;
        private Vector3 launchDirection = Vector3.zero;
        private float launchPower = 0f;
        private float launchAngle;
        private OrbitalMover orbitalMover;
        private Vector3 startingPosition;
        private bool resetPosition = false;

        // Arrow
        [SerializeField] private GameObject arrowPrefab;
        private GameObject arrowInstance;

        // Update position
        private Vector3 frameForce;

        // Out of bounds
        private float outOfBoundsTimeCap = 5f;
        private float outOfBoundsTime = 0f;
        private ScreenBounds screenBounds;
        [SerializeField] private Canvas outOfBoundsCanvas;
        private Vector3 canvasStartingPosition;
        [SerializeField] private TMP_Text outOfBoundsText;

        private void Awake()
        {
            orbitalMover = GetComponent<OrbitalMover>();
            startingPosition = transform.position;
            outOfBoundsCanvas.gameObject.SetActive(false);
        }

        private void Start()
        {
            screenBounds = GetBounds();
            ResetLaunch();
            canvasStartingPosition = outOfBoundsCanvas.transform.position;
        }

        private void FixedUpdate()
        {
            frameForce = Vector3.zero;

            if (resetPosition)
            {
                transform.position = startingPosition;
                resetPosition = false;
            }

            if (!isHeld)
            {
                Vector3 orbitForce = orbitalMover.GetOrbitForce((int)Mathf.Sign(launchAngle));
                frameForce += orbitForce;

                if (launchPower > 0)
                {
                    Vector3 launchForce = launchDirection * launchPower * Time.fixedDeltaTime;
                    frameForce += launchForce;
                    launchPower -= 0.01f * Time.timeScale;
                }
                else
                {
                    ResetLaunch();
                }
            }

            if (!float.IsNaN(frameForce.x) && !float.IsNaN(frameForce.y) && frameForce != Vector3.zero)
                transform.position += frameForce;
        }

        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 worldPoint = GetMousePosition();
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0);
                if (hit && hit.transform.gameObject.Equals(this.gameObject))
                {
                    Time.timeScale = 0.25f;
                    isHeld = true;
                    arrowInstance = Instantiate(arrowPrefab, transform.position, Quaternion.identity, this.transform);
                }
            }

            if (Input.GetMouseButtonUp(0) && isHeld)
            {
                isHeld = false;
                Time.timeScale = 1f;
                Destroy(arrowInstance);
                arrowInstance = null;
                launchAngle = Vector3.SignedAngle(transform.position, launchDirection, Vector3.forward);
            }

            if (isHeld)
            {
                Launch();
                AdaptLaunchArrow();
            }

            if (CheckIfOutsideScreen(transform.position, 1f))
            {
                outOfBoundsTime += Time.deltaTime;
                outOfBoundsCanvas.gameObject.SetActive(true);
                UpdateOutOfBoundText();
            }
            else
            {
                outOfBoundsCanvas.gameObject.SetActive(false);
                outOfBoundsTime = 0f;
            }

            if (outOfBoundsTime >= outOfBoundsTimeCap)
            {
                ResetLaunch();
                outOfBoundsCanvas.gameObject.SetActive(false);
                outOfBoundsTime = 0f;
                resetPosition = true;
                // transform.position = startingPosition;
            }

            if (outOfBoundsCanvas.gameObject.activeSelf)
                outOfBoundsCanvas.transform.position = startingPosition + canvasStartingPosition; 
        }

        private void Launch()
        {
            Vector3 worldPoint = GetMousePosition();
            launchDirection = (transform.position - worldPoint).normalized;
            launchPower = Mathf.Clamp((transform.position - worldPoint).magnitude * 2f, 0f, 6f);
        }

        private void AdaptLaunchArrow()
        {
            float launchSignedAngle = -Vector3.SignedAngle(launchDirection, transform.up, Vector3.forward);
            arrowInstance.transform.rotation = Quaternion.AngleAxis(launchSignedAngle, Vector3.forward);
            arrowInstance.transform.localScale = new Vector3(arrowInstance.transform.localScale.x,
                launchPower * .25f, arrowInstance.transform.localScale.z);
        }

        private Vector3 GetMousePosition()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return new Vector3(mousePos.x, mousePos.y, transform.position.z);
        }

        private void ResetLaunch()
        {
            launchDirection = Vector3.zero;
            launchPower = 0f;
        }
                private bool CheckIfOutsideScreen(Vector3 position, float multiplier = 1f)
        {
            return (position.x < screenBounds.lowerX * multiplier || position.x > screenBounds.upperX * multiplier ||
                    position.y < screenBounds.lowerY * multiplier || position.y > screenBounds.upperY * multiplier);
        }

        private ScreenBounds GetBounds()
        {
            float screenHeight = Camera.main.orthographicSize * 2f;
            float screenWidth = Camera.main.aspect * screenHeight;
            float lowerX = -screenWidth / 2f;
            float upperX = screenWidth / 2f;
            float lowerY = -screenHeight / 2f;
            float upperY = screenHeight / 2f;
            return new ScreenBounds(lowerX, upperX, lowerY, upperY);
        }

        private void UpdateOutOfBoundText()
        {
            int timeToReload = Mathf.FloorToInt(outOfBoundsTimeCap - outOfBoundsTime) + 1;
            outOfBoundsText.text = $"Back in {timeToReload}..";
        }
    }
}