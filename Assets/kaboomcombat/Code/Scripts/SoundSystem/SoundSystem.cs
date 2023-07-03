
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


        public bool IsSoundPlaying()
        {
            if(soundSource.isPlaying) { return true; }
            else { return false; }
        }


        public void PlaySound(AudioClip clip)
        {
            soundSource.PlayOneShot(clip);
        }

        public void PlaySound(Sounds sound)
        {
            soundSource.PlayOneShot(soundList[(int)sound]);
        }

        public void PlayMusic(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }

        public void PlayMusic(Music music)
        {
            musicSource.clip = musicList[(int)music];
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }
    }
}
