using UnityEngine;

namespace kaboomcombat
{
    public class PowerupMoveSpeed : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Player>().SetPowerUpMoveSpeed();
                Destroy(gameObject);
            }
        }
    }
}
