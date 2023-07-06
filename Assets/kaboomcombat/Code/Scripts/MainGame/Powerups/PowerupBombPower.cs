// PowerupBombPower class
// ====================================================================================================================
// Handles the bomb power powerup


using UnityEngine;


namespace kaboomcombat
{
    public class PowerupBombPower : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Player>().IncrementBombPower();
            }
        }
    }
}
