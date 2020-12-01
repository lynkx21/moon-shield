using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class SFXManager : MonoBehaviour
    {
        private AudioSource sfxSource;
        private GameObject soundGameObject;

        // Sounds
        public AudioClip enemyExplosionClip;
        public AudioClip earthExplosionClip;
        public AudioClip earthHit;

        public void Start()
        {
            // SFX
            soundGameObject = new GameObject("Sound Output");
            sfxSource = soundGameObject.AddComponent<AudioSource>();
        }
        public void PlaySound(AudioClip audioClip)
        {
            sfxSource.PlayOneShot(audioClip);
        }
    }
}