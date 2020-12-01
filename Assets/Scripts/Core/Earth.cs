using System;
using UnityEngine;
using Audio;

namespace Core
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Earth : MonoBehaviour
    {
        private CircleCollider2D collider;
        [SerializeField] private float gravityRadius = 5f;
        public float gravityMaxDistance;
        public float mass = 1f;
        public int hitPoints = 1;
        public event Action<int> OnTakeDamage;
        public event Action OnGameOver;

        // Audio
        [SerializeField] SFXManager sfxManager;

        private void Awake()
        {
            collider = GetComponent<CircleCollider2D>();
            gravityMaxDistance = gravityRadius + collider.radius;
        }

        private void Start()
        {
            sfxManager = FindObjectOfType<SFXManager>();
        }


        void Update()
        {
            if (hitPoints == 0)
            {
                GameOver();
            }

        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, gravityMaxDistance);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                sfxManager.PlaySound(sfxManager.earthHit);
                Destroy(collision.gameObject);
                hitPoints--;
            }
            else if (collision.gameObject.tag == "Moon")
            {
                Destroy(collision.gameObject);
                hitPoints = 0;
            }

            if (OnTakeDamage != null)
            {
                OnTakeDamage.Invoke(hitPoints);
            }
        }

        private void GameOver()
        {
            if (OnGameOver != null)
            {
                OnGameOver.Invoke();
            }
            sfxManager.PlaySound(sfxManager.earthExplosionClip);
            Destroy(gameObject);
        }
    }
}