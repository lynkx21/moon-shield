using System;
using UnityEngine;
using Audio;

namespace Core
{ 
    // [RequireComponent(typeof(AudioSource))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected float speed = 3f;
        [SerializeField] protected int points = 50;
        protected Vector3 destination = Vector3.zero;
        public event Action<int> OnDeath;

        // Audio
        [SerializeField] protected SFXManager sfxManager;

        protected virtual void Start()
        {
            sfxManager = FindObjectOfType<SFXManager>();
        }

        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                sfxManager.PlaySound(sfxManager.enemyExplosionClip);
            }
            
            Vector3 direction = GetDirection();
            transform.position += direction * speed * Time.deltaTime;
        }

        public virtual Vector3 GetDirection()
        {
            Vector3 straightDirection = (destination - transform.position).normalized;
            return straightDirection;
        }

        public virtual void Die()
        {

            sfxManager.PlaySound(sfxManager.enemyExplosionClip);
            Destroy(this.gameObject);
            if (OnDeath != null)
            {
                OnDeath.Invoke(points);
            }
        }

        /*
        public void OnDestroy()
        {
            if (OnDeath != null)
            {
                OnDeath.Invoke(points);
            }
        }
        */
    } 
}