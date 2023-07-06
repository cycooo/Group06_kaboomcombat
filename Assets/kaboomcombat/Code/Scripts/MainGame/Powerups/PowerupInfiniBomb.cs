// PowerupInfiniBomb class
// ====================================================================================================================
// Handles the infinite bombs powerup


using UnityEngine;


namespace kaboomcombat
{
    public class PowerupInfiniBomb : MonoBehaviour
    {
        private Player player;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                player = other.gameObject.GetComponent<Player>();
                player.SetPowerupInfiniBomb(10f);
            }
        }
    }
}
