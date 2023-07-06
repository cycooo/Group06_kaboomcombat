// Powerup Class
// ====================================================================================================================
// Base class for powerups


using UnityEngine;


namespace kaboomcombat
{
    public class Powerup : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // If the player collides with the powerup, play the powerup collect sound
            if(other.gameObject.CompareTag("Player"))
            {
                SoundSystem.instance.PlaySound(Sounds.UI_SELECT);
            }

            // Destroy the powerup when something collides with it
            Kill();
        }

        
        // Function to destroy the powerup
        private void Kill()
        {
            // Subtract one from the powerupCounter
            FindObjectOfType<SessionManager>().powerupCounter--;
            LevelManager.DestroyObject(gameObject);
            Destroy(gameObject);
        }
    }
}
