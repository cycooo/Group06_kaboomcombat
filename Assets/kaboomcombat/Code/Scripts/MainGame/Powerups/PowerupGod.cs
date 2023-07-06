// PowerupGod class
// ====================================================================================================================
// Handles the invulnerability powerup


using UnityEngine;


namespace kaboomcombat
{
    public class PowerupGod : MonoBehaviour
    {
        private Player player;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                player = other.gameObject.GetComponent<Player>();
                player.SetPowerupGod(10f);
            }
        }
    }
}
