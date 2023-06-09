// Explosion class
// ====================================================================================================================
// Mini-class that handles explosion objects and what happens when they hit certain other objects


using UnityEngine;


namespace kaboomcombat
{
    public class Explosion : MonoBehaviour
    {
        private bool active = true;
        public Player ownerPlayer;

        void Start()
        {
            // "Deactivate" the explosion after 0.5 seconds so it doesn't deal damage when fading out
            Invoke("Deactivate", 0.5f);
            // Remove the explosion after one second
            Invoke("Kill", 1f);
        }


        void OnTriggerEnter(Collider other)
        {
            // Check if the explosion is still active
            if(active)
            {
                // If the object is a bomb, detonate it as well
                // This makes nice chain reaction explosions
                if (other.gameObject.CompareTag("Bomb"))
                {
                    other.gameObject.GetComponent<BombController>().Explode();
                }
                else if(other.gameObject.CompareTag("Player"))
                {
                    if(!other.gameObject.GetComponent<Player>().god)
                    {
                        other.gameObject.GetComponent<Player>().Kill();
                        if(other.gameObject != ownerPlayer.gameObject)
                        {
                            ownerPlayer.IncrementKills();
                            ownerPlayer.IncrementBombPower();
                        }
                    }
                }
                // If the object is not indestructible, destroy it
                // This way, anything caught in the bomb's explosion is destroyed (Players, powerups, brick walls etc.)
                else if (!other.gameObject.CompareTag("Indestructible"))
                {
                    LevelManager.DestroyObject(other.gameObject);
                }
            }
        }


        private void Deactivate()
        {
            active = false;
        }


        private void Kill()
        {
            Destroy(gameObject);
        }
    }
}
