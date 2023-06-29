using UnityEngine;


namespace kaboomcombat
{
    public class MusicPlayer : MonoBehaviour
    {
        private AudioSource audioSource;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayMusic()
        {
            audioSource.Play();
        }

        public void StopMusic()
        {
            audioSource.Stop();
        }
    }

}
