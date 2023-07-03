// LevelManager Class
// ====================================================================================================================
// Handles everything to do with the level itself, ex. level generation, randomness, adding/removing objects etc.


using System.Collections.Generic;
using UnityEngine;


namespace kaboomcombat
{
    public class LevelManager : MonoBehaviour
    {
        // Level variables
        private int levelWidth = 13;
        private int levelHeight = 11;
        private int randomness = 7;

        // References
        private SessionManager sessionManager;
        public static GameObject[,] levelMatrix;
        private List<GameObject> objectList;


        void Start()
        {
            // Get references to SessionManager and objectList
            sessionManager = GetComponent<SessionManager>();
            objectList = sessionManager.objectList;

            // Run the function that generates the level
            GenerateLevel(); 
        }


        // Helper function that converts world coordinates to levelMatrix coordinates.
        public static Vector3 WorldToMatrixPosition(Vector3 source)
        {
            // Round the float world position to an int, which (hopefully) results in the appropriate matrix position
            int xPos = Mathf.RoundToInt(source.x);
            int zPos = Mathf.RoundToInt(source.z);

            // Return the new vector
            return new Vector3(xPos, 0f, zPos);
        }


        // Helper function that searches a given tile of the level and returns it's contents (null if empty)
        public static GameObject SearchLevelTile(Vector3 tilePosition)
        {
            // Convert world position to levelMatrix position
            Vector3 matrixPosition = WorldToMatrixPosition(tilePosition);

            return (levelMatrix[(int)matrixPosition.x, (int)matrixPosition.z]);
        }


        public static Vector3 GetRandomMatrixPosition()
        {
            Vector3 randomPos = new Vector3(Random.Range(0, 13), 0f, Random.Range(0, 11));

            return randomPos;
        }


        // Function to instantiate a prefab and place it on the levelMatrix
        public static GameObject SpawnObject(GameObject prefab, Vector3 spawnPosition)
        {
            try
            {
                // Convert the world position to levelMatrix position
                Vector3 matrixPosition = WorldToMatrixPosition(spawnPosition);

                // Instantiate the object at the given positions
                GameObject spawnedObject = Instantiate(prefab, matrixPosition, prefab.transform.rotation);

                if (spawnedObject != null)
                {
                    // Add the object to the levelMatrix 
                    levelMatrix[(int)matrixPosition.x, (int)matrixPosition.z] = prefab;
                }

                return spawnedObject;
            }
            catch (System.NullReferenceException)
            {
                Debug.LogError("[SpawnObject] Prefab of object to spawn is null!");
                return null;
            }
        }


        // Function to Destroy an object and remove it from the levelMatrix
        public static void DestroyObject(GameObject target)
        {
            try
            {
                // Destroy the object on the map
                Destroy(target);
                // Convert world position to levelMatrix position
                Vector3 matrixPosition = WorldToMatrixPosition(target.transform.position);

                // Remove the object from the levelMatrix 
                levelMatrix[(int)matrixPosition.x, (int)matrixPosition.z] = null;
            }
            catch (System.NullReferenceException)
            {
                Debug.LogError("[DestroyObject] Target object is null!");
            }
        }


        // Function that generates a random level layout, then calls buildLevel()
        private void GenerateLevel()
        {
            levelMatrix = new GameObject[levelWidth, levelHeight];

            // For loop to build the level matrix
            // Start with the vertical axis, as such i and j are reversed
            for(int i = 0; i < levelHeight; i++)
            {
                for(int j = 0; j < levelWidth; j++)
                {
                    // Assign a new random seed
                    int randomSeed = Random.Range(0, 10);

                    // Place stone blocks every other block, else place brick blocks randomly
                    if(i % 2 != 0  &&  j % 2 != 0)
                    {
                        // Place object with index 0 (Stone Block)
                        levelMatrix[j, i] = objectList[0];                        
                    }
                    else if(randomSeed < randomness)
                    {
                        // Place object with index 1 (Brick Block)
                        levelMatrix[j, i] = objectList[1];
                    }
                }
            }

            // Remove corners
            // There has to be a better way to do this, but I'm too lazy to find it
            levelMatrix[0,0] = null;
            levelMatrix[1,0] = null;
            levelMatrix[0,1] = null;
            levelMatrix[levelWidth-1,0] = null;
            levelMatrix[levelWidth-2,0] = null;
            levelMatrix[levelWidth-1,1] = null;
            levelMatrix[0,levelHeight-1] = null;
            levelMatrix[1,levelHeight-1] = null;
            levelMatrix[0,levelHeight-2] = null;
            levelMatrix[levelWidth-1,levelHeight-1] = null;
            levelMatrix[levelWidth-2,levelHeight-1] = null;
            levelMatrix[levelWidth-1,levelHeight-2] = null;

            // Call a function which will actually spawn the objects according to the matrix we just built
            BuildLevel();
        }

        // Function that places gameObjects according to the levelMatrix
        private void BuildLevel()
        {
            // Once again, i and j are reversed because we traverse the matrix rows first
            for(int i = 0; i < levelHeight; i++)
            {
                for(int j = 0; j < levelWidth; j++)
                {
                    if(levelMatrix[j, i] != null)
                    {
                        Instantiate(levelMatrix[j, i], new Vector3(j, 0f, i), levelMatrix[j, i].transform.rotation);
                    }
                }
            }
        }
    }
}