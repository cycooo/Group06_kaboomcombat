using System.Collections;
using System.Collections.Generic;
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
