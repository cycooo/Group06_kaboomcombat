using UnityEngine;

namespace kaboomcombat
{
    public class PowerupMoveSpeed : MonoBehaviour
    {
        private Player player;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                player = other.gameObject.GetComponent<Player>();
                player.SetPowerupMoveSpeed(1.2f, 10f);
            }
        }
    }
}
