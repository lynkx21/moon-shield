using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class MoonCrusher : MonoBehaviour
    {
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Enemy")
            {
                collision.gameObject.GetComponent<Enemy>().Die();
            }
        }
    }
}