// SoundSystem Class
// ====================================================================================================================
// Singleton class that handles playing music and sounds


using System.Collections.Generic;
using UnityEngine;


namespace kaboomcombat
{
    public class SoundSystem : MonoBehaviour
    {
        public static SoundSystem instance;

        [SerializeField] private AudioSource musicSource, soundSource;

        [SerializeField] private List<AudioClip> musicList = new List<AudioClip>();
        [SerializeField] private List<AudioClip> soundList = new List<AudioClip>();

        // Make this a singleton
        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        // Function to check if a sound is playing
        public bool IsSoundPlaying()
        {
            if(soundSource.isPlaying) { return true; }
            else { return false; }
        }


        // Function to play a sound, taking in the audioclip directly
        public void PlaySound(AudioClip clip)
        {
            soundSource.PlayOneShot(clip);
        }


        // Function to play a sound, taking in an entry in the Sounds enum
        public void PlaySound(Sounds sound)
        {
            soundSource.PlayOneShot(soundList[(int)sound]);
        }


        // Function to stop all sounds
        public void StopAllSounds()
        {
            soundSource.Stop();
        }


        // Function to play music, taking in the audioclip directly
        public void PlayMusic(AudioClip clip)
        {
            if(musicSource.clip == clip && musicSource.isPlaying)
            {
                return;
            }

            musicSource.clip = clip;
            musicSource.Play();
        }


        // Function to play music, taking in an entry in the Music enum
        public void PlayMusic(Music music)
        {
            musicSource.clip = musicList[(int)music];
            musicSource.Play();
        }


        // Function to stop music
        public void StopMusic()
        {
            musicSource.Stop();
        }
    }
}
