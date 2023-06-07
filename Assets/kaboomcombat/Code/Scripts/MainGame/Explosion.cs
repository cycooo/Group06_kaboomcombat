// Explosion class
// =====================================================================================================================
// Mini-class that handles explosion objects and what happens when they hit certain other objects


using UnityEngine;


namespace kaboomcombat
{
    public class Explosion : MonoBehaviour
{
    void Start()
    {
        // Remove the explosion after one second
        Invoke("Kill", 1f);
    }


    void OnTriggerEnter(Collider other)
    {
        // If the object is a bomb, detonate it as well
        // This makes nice chain reaction explosions
        if (other.gameObject.CompareTag("Bomb"))
        {
            other.gameObject.GetComponent<BombController>().Explode();
        }
        // If the object is not indestructible, destroy it
        // This way, anything caught in the bomb's explosion is destroyed (Players, powerups, brick walls etc.)
        else if (!other.gameObject.CompareTag("Indestructible"))
        {
            LevelManager.DestroyObject(other.gameObject);
        }
    }


    private void Kill()
    {
        Destroy(gameObject);
    }
}
}
