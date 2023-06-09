// PlayerRagdoll class
// ====================================================================================================================
// Handles the flying playermodel when a player gets blown up


using UnityEngine;


namespace kaboomcombat
{
    public class PlayerRagdoll : MonoBehaviour
    {
        public Transform playermodelContainer;
        private Rigidbody rb;

        void Start()
        {
            // Make the playermodel fly upwards in a random direction
            rb = GetComponent<Rigidbody>();

            float xForce = Random.Range(-400f, 400f);
            float zForce = Random.Range(-400f, 400f);

            float xTorque = Random.Range(-120f, 90f);
            float yTorque = Random.Range(-90, 90f);
            float zTorque = Random.Range(-120f, 120f);

            rb.AddForce(new Vector3(xForce, 800f, zForce));
            rb.AddTorque(new Vector3(xTorque, yTorque, zTorque));

            // Destroy the playermodel after 3 seconds
            Invoke("Kill", 3f);
        }

        private void Kill()
        {
            Destroy(gameObject);
        }

        public void AddPlayermodel(GameObject playermodel)
        {
            Instantiate(playermodel, playermodelContainer);
        }
    }

}
