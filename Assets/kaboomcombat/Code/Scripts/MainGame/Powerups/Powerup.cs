using UnityEngine;

namespace kaboomcombat
{
    public class Powerup : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                SoundSystem.instance.PlaySound(Sounds.UI_SELECT);
            }

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
