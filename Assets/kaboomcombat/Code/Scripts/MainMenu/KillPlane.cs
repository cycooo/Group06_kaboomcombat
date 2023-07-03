// Killplane class
// ====================================================================================================================
// Micro class that destroys any object that touches this object (i.e. a killplane)


using UnityEngine;


public class KillPlane : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
