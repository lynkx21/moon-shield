using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class SoundtrackManager : MonoBehaviour
    {
        public static SoundtrackManager instance = null;
        private AudioSource soundtrackSource;
        
        // Sounds
        public AudioClip soundtrack;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }

            if (instance == this)
                return;

            Destroy(gameObject);
        }

        public void Start()
        {
            // Soundtrack
            soundtrackSource = gameObject.AddComponent<AudioSource>();
            soundtrackSource.clip = soundtrack;
            soundtrackSource.volume = .85f;
            PlaySoundtrack();
        }

        public void PlaySoundtrack()
        {
            soundtrackSource.Play();
        }
    }
}