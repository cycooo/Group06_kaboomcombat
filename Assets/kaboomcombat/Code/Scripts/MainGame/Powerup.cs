using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kaboomcombat
{
    public class Powerup : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            SoundSystem.instance.PlaySound(Sounds.UI_SELECT);
            Kill();
        }

        private void Kill()
        {
            FindObjectOfType<SessionManager>().powerupCounter--;
            LevelManager.DestroyObject(gameObject);
            Destroy(gameObject);
        }
    }
}
