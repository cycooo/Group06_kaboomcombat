using System.Collections.Generic;
using UnityEngine;

namespace kaboomcombat
{
    public class SoundSystem : MonoBehaviour
    {
        public List<AudioClip> sounds = new List<AudioClip>();

        private AudioSource audioSource;


        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        
        public void PlaySound(Sounds sound)
        {
            audioSource.clip = sounds[(int)sound];
            audioSource.Play();
        }
    }
}
