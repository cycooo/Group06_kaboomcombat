// Class that plays an audio clip on spawn

using UnityEngine;

namespace kaboomcombat
{
    public class PlaySoundOnStart : MonoBehaviour
    {
        [SerializeField] private AudioClip clip;

        void Start()
        {
            SoundSystem.instance.PlaySound(clip);
        }
    }

}
