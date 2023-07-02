using UnityEngine;

namespace kaboomcombat
{
    public class PowerupGod : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Player>().SetPowerUpGod();
                Destroy(gameObject);
            }
        }
    }
}
