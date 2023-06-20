// BombController class
// ====================================================================================================================
// Handles bomb parameters (timer, bombPower etc.) and explosions

using UnityEngine;


namespace kaboomcombat
{
    public class BombController : MonoBehaviour
    {
        // Bomb variables
        public int bombPower = 2;
        public float bombTimer = 3f;
        public bool autoStart = true;
        public Player ownerPlayer;

        // References
        public GameObject explosionPrefab;
        public GameObject explosionEndPrefab;


        void Start()
        {
            // If autoStart is set to true, start the bomb's timer right away
            if(autoStart)
            {
                StartTimer();
            }
        }

        // Function to start the bomb timer manually if autoStart is false
        private void StartTimer()
        {
            Invoke("Explode", bombTimer);
        }


        // Ensure that the bomb is removed from the level matrix if it is destroyed by means other than the LevelManager function
        // TODO: Make this less bad; when destroyed correctly, bombs call the destroy function twice, just to make sure they are,
        // in fact, being destroyed properly
        private void OnDestroy()
        {
            LevelManager.DestroyObject(gameObject);
        }


        private void InstantiateExplosion(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject explosionInstance = Instantiate(prefab, position, rotation);
            explosionInstance.GetComponent<Explosion>().ownerPlayer = ownerPlayer;
        }


        // Function that handles the bomb's explosion
        public void Explode()
        {
            // Store the current position for easier access
            Vector3 currentPos = transform.position;

            // Spawn the middle explosion separately so that it doesn't overlap
            InstantiateExplosion(explosionPrefab, currentPos, explosionPrefab.transform.rotation);

            // Define RaycastHits for every direction
            RaycastHit hitLeft;
            RaycastHit hitRight;
            RaycastHit hitUp;
            RaycastHit hitDown;


            /*
            * Cast rays to see if the bomb hits an obstacle
            * If yes, then only InstantiateExplosion explosion objects until we hit the object
            * If no, InstantiateExplosion all explosion objects according to bombPower
            *
            * The formula for instantiating the explosion objects is as follows:
            * Start from the bomb x/z position, then subtract/add 1 to it to remove the explosion on top of the bomb,
            * otherwise it would be duplicated by each direction.
            * Iterate until we reach the collision's x/z position;
            * If the collision object is destructible, we also place an explosion on top of it, to destroy it.
            * Otherwise, stop before placing an explosion on top of the object, as it cannot be destroyed.
            * 
            * Also, if we are on the last iteration of the for loop, place a different explosion object to add variety.
            * 
            * All positional coordinates are converted to MatrixPositions beforehand, so as to obtain clean coordinates
            * that match the ones in the levelArray matrix.
            * This is so we can easily keep track of what square is occupied and by what etc.
            */


            // Convert the bomb's float position to matrix position
            Vector3 matrixPosition = LevelManager.WorldToMatrixPosition(currentPos);

            // Left Raycast
            // Check if something is in the way of the bomb
            if(Physics.Raycast(currentPos, transform.TransformDirection(Vector3.left), out hitLeft, bombPower))
            {
                // Convert the float coordinates of the hit object to matrix coordinates
                Vector3 hitLeftMatrixPos = LevelManager.WorldToMatrixPosition(hitLeft.transform.position);

                // Check if the collision object is indestructible
                if(hitLeft.collider.gameObject.CompareTag("Indestructible"))
                {
                    // Iterate through all positions from the bomb's position to the target's, and place explosion objects accordingly.
                    // Subtract 1 from i so as to not place an explosion object on top of the bomb object.
                    for(int i = (int) matrixPosition.x - 1; i > hitLeftMatrixPos.x; i--)
                    {
                        // Vector3 to store the position of the current iteration
                        Vector3 explosionPos = new Vector3(i, 0f, hitLeftMatrixPos.z);
                        // If this is the last iteration of the for loop, place an explosionEnd object
                        if(i == hitLeftMatrixPos.x + 1)
                        {
                            InstantiateExplosion(explosionEndPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                        // else place a normal explosion object
                        else
                        {
                            InstantiateExplosion(explosionPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                    }
                }
                // If the collision object is not indestructible, place explosions up to and including the object itself,
                // so as to destroy it.
                else
                {
                    // Iterate through all positions from the bomb's position to the target's, and place explosion objects accordingly.
                    // Subtract 1 from i so as to not place an explosion object on top of the bomb object.
                    for(int i = (int) matrixPosition.x - 1; i >= hitLeftMatrixPos.x; i--)
                    {
                        // Vector3 to store the position of the current iteration
                        Vector3 explosionPos = new Vector3(i, 0f, hitLeftMatrixPos.z);
                        // If this is the last iteration of the for loop, place an explosionEnd object
                        if(i == hitLeftMatrixPos.x)
                        {
                            InstantiateExplosion(explosionEndPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                        // else place a normal explosion object
                        else
                        {
                            InstantiateExplosion(explosionPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                    }
                }
            }
            // If the bomb doesn't reach anything, spawn the full amount of explosion objects, according to bombPower.
            else
            {
                // Iterate through all positions from the bomb's position to the max distance of the explosion, which we obtain by
                // subtracting bombPower from the bomb's current position.
                // Subtract 1 from i so as to not place an explosion object on top of the bomb object.
                for(int i = (int) matrixPosition.x - 1; i >= currentPos.x - bombPower; i--)
                {
                    // Vector3 to store the position of the current iteration
                    Vector3 explosionPos = new Vector3(i, 0f, currentPos.z);
                    // If this is the last iteration of the for loop, place an explosionEnd object
                    if(i == currentPos.x - bombPower)
                    {
                        InstantiateExplosion(explosionEndPrefab, explosionPos, explosionPrefab.transform.rotation);
                    }
                    // else place a normal explosion object
                    else
                    {
                        InstantiateExplosion(explosionPrefab, explosionPos, explosionPrefab.transform.rotation);
                    }
                }
            }

            // Right Raycast
            // Check if something is in the way of the bomb
            if(Physics.Raycast(currentPos, transform.TransformDirection(Vector3.right), out hitRight, bombPower))
            {
                // Convert the float coordinates of the hit object to matrix coordinates
                Vector3 hitRightMatrixPos = LevelManager.WorldToMatrixPosition(hitRight.transform.position);

                // Check if the collision object is indestructible
                if(hitRight.collider.gameObject.CompareTag("Indestructible"))
                {
                    // Iterate through all positions from the bomb's position to the target's, and place explosion objects accordingly.
                    // Add 1 to i so as to not place an explosion object on top of the bomb object.
                    for(int i = (int) matrixPosition.x + 1; i < hitRightMatrixPos.x; i++)
                    {
                        // Vector3 to store the position of the current iteration
                        Vector3 explosionPos = new Vector3(i, 0f, hitRightMatrixPos.z);
                        // If this is the last iteration of the for loop, place an explosionEnd object
                        if(i == hitRightMatrixPos.x - 1)
                        {
                            InstantiateExplosion(explosionEndPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                        // else place a normal explosion object
                        else
                        {
                            InstantiateExplosion(explosionPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                    }
                }
                // If the collision object is not indestructible, place explosions up to and including the object itself,
                // so as to destroy it.
                else
                {
                    // Iterate through all positions from the bomb's position to the target's, and place explosion objects accordingly.
                    // Add 1 to i so as to not place an explosion object on top of the bomb object.
                    for(int i = (int) matrixPosition.x + 1; i <= hitRightMatrixPos.x; i++)
                    {
                        // Vector3 to store the position of the current iteration
                        Vector3 explosionPos = new Vector3(i, 0f, hitRightMatrixPos.z);
                        // If this is the last iteration of the for loop, place an explosionEnd object
                        if(i == hitRightMatrixPos.x)
                        {
                            InstantiateExplosion(explosionEndPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                        // else place a normal explosion object
                        else
                        {
                            InstantiateExplosion(explosionPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                    }
                }
            }
            // If the bomb doesn't reach anything, spawn the full amount of explosion objects, according to bombPower.
            else
            {
                // Iterate through all positions from the bomb's position to the max distance of the explosion, which we obtain by
                // subtracting bombPower from the bomb's current position.
                // Add 1 to i so as to not place an explosion object on top of the bomb object.
                for(int i = (int) matrixPosition.x + 1; i <= currentPos.x + bombPower; i++)
                {
                    // Vector3 to store the position of the current iteration
                    Vector3 explosionPos = new Vector3(i, 0f, currentPos.z);
                    // If this is the last iteration of the for loop, place an explosionEnd object
                    if(i == currentPos.x + bombPower)
                    {
                        InstantiateExplosion(explosionEndPrefab, explosionPos, explosionPrefab.transform.rotation);
                    }
                    // else place a normal explosion object
                    else
                    {
                        InstantiateExplosion(explosionPrefab, explosionPos, explosionPrefab.transform.rotation);
                    }
                }
            }

            // Up Raycast
            // Check if something is in the way of the bomb
            if(Physics.Raycast(currentPos, transform.TransformDirection(Vector3.forward), out hitUp, bombPower))
            {
                // Convert the float coordinates of the hit object to matrix coordinates
                Vector3 hitUpMatrixPos = LevelManager.WorldToMatrixPosition(hitUp.transform.position);

                // Check if the collision object is indestructible
                if(hitUp.collider.gameObject.CompareTag("Indestructible"))
                {
                    // Iterate through all positions from the bomb's position to the target's, and place explosion objects accordingly.
                    // Add 1 to i so as to not place an explosion object on top of the bomb object.
                    for(int i = (int) matrixPosition.z + 1; i < hitUpMatrixPos.z; i++)
                    {
                        // Vector3 to store the position of the current iteration
                        Vector3 explosionPos = new Vector3(hitUpMatrixPos.x, 0f, i);
                        // If this is the last iteration of the for loop, place an explosionEnd object
                        if(i == hitUpMatrixPos.z - 1)
                        {
                            InstantiateExplosion(explosionEndPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                        // else place a normal explosion object
                        else
                        {
                            InstantiateExplosion(explosionPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                    }
                }
                // If the collision object is not indestructible, place explosions up to and including the object itself,
                // so as to destroy it.
                else
                {
                    // Iterate through all positions from the bomb's position to the target's, and place explosion objects accordingly.
                    // Add 1 to i so as to not place an explosion object on top of the bomb object.
                    for(int i = (int) matrixPosition.z + 1; i <= hitUpMatrixPos.z; i++)
                    {
                        // Vector3 to store the position of the current iteration
                        Vector3 explosionPos = new Vector3(hitUpMatrixPos.x, 0f, i);
                        // If this is the last iteration of the for loop, place an explosionEnd object
                        if(i == hitUpMatrixPos.z)
                        {
                            InstantiateExplosion(explosionEndPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                        // else place a normal explosion object
                        else
                        {
                            InstantiateExplosion(explosionPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                    }
                }
            }
            // If the bomb doesn't reach anything, spawn the full amount of explosion objects, according to bombPower.
            else
            {
                // Iterate through all positions from the bomb's position to the max distance of the explosion, which we obtain by
                // subtracting bombPower from the bomb's current position.
                // Add 1 to i so as to not place an explosion object on top of the bomb object.
                for(int i = (int) matrixPosition.z + 1; i <= currentPos.z + bombPower; i++)
                {
                    // Vector3 to store the position of the current iteration
                    Vector3 explosionPos = new Vector3(currentPos.x, 0f, i);
                    // If this is the last iteration of the for loop, place an explosionEnd object
                    if(i == currentPos.z + bombPower)
                    {
                        InstantiateExplosion(explosionEndPrefab, explosionPos, explosionPrefab.transform.rotation);
                    }
                    // else place a normal explosion object
                    else
                    {
                        InstantiateExplosion(explosionPrefab, explosionPos, explosionPrefab.transform.rotation);
                    }
                }
            }

            // Down Raycast
            // Check if something is in the way of the bomb
            if(Physics.Raycast(currentPos, transform.TransformDirection(Vector3.back), out hitDown, bombPower))
            {
                // Convert the float coordinates of the hit object to matrix coordinates
                Vector3 hitDownMatrixPos = LevelManager.WorldToMatrixPosition(hitDown.transform.position);

                // Check if the collision object is indestructible
                if(hitDown.collider.gameObject.CompareTag("Indestructible"))
                {
                    // Iterate through all positions from the bomb's position to the target's, and place explosion objects accordingly.
                    // Subtract 1 from i so as to not place an explosion object on top of the bomb object.
                    for(int i = (int) matrixPosition.z - 1; i > hitDownMatrixPos.z; i--)
                    {
                        // Vector3 to store the position of the current iteration
                        Vector3 explosionPos = new Vector3(hitDownMatrixPos.x, 0f, i);
                        // If this is the last iteration of the for loop, place an explosionEnd object
                        if(i == hitDownMatrixPos.z + 1)
                        {
                            InstantiateExplosion(explosionEndPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                        // else place a normal explosion object
                        else
                        {
                            InstantiateExplosion(explosionPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                    }
                }
                // If the collision object is not indestructible, place explosions up to and including the object itself,
                // so as to destroy it.
                else
                {
                    // Iterate through all positions from the bomb's position to the target's, and place explosion objects accordingly.
                    // Subtract 1 from i so as to not place an explosion object on top of the bomb object.
                    for(int i = (int) matrixPosition.z - 1; i >= hitDownMatrixPos.z; i--)
                    {
                        // Vector3 to store the position of the current iteration
                        Vector3 explosionPos = new Vector3(hitDownMatrixPos.x, 0f, i);
                        // If this is the last iteration of the for loop, place an explosionEnd object
                        if(i == hitDownMatrixPos.z)
                        {
                            InstantiateExplosion(explosionEndPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                        // else place a normal explosion object
                        else
                        {
                            InstantiateExplosion(explosionPrefab, explosionPos, explosionPrefab.transform.rotation);
                        }
                    }
                }
            }
            // If the bomb doesn't reach anything, spawn the full amount of explosion objects, according to bombPower.
            else
            {
                // Iterate through all positions from the bomb's position to the max distance of the explosion, which we obtain by
                // subtracting bombPower from the bomb's current position.
                // Subtract 1 from i so as to not place an explosion object on top of the bomb object.
                for(int i = (int) matrixPosition.z - 1; i >= currentPos.z - bombPower; i--)
                {
                    // Vector3 to store the position of the current iteration
                    Vector3 explosionPos = new Vector3(currentPos.x, 0f, i);
                    // If this is the last iteration of the for loop, place an explosionEnd object
                    if(i == currentPos.z - bombPower)
                    {
                        InstantiateExplosion(explosionEndPrefab, explosionPos, explosionPrefab.transform.rotation);
                    }
                    // else place a normal explosion object
                    else
                    {
                        InstantiateExplosion(explosionPrefab, explosionPos, explosionPrefab.transform.rotation);
                    }
                }
            }

            // Destroy the bomb object after we're done
            LevelManager.Destroy(gameObject);
        }
    }
}
