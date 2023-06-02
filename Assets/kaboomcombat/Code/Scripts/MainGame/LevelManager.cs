using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kaboomcombat
{
    public class LevelManager : MonoBehaviour
    {
        private int levelWidth = 13;
        private int levelHeight = 11;

        private int randomness = 0;

        // References
        private SessionManager sessionManager;

        public static GameObject[,] levelMatrix;

        private List<GameObject> objectList;


        void Start()
        {
            sessionManager = GetComponent<SessionManager>();
            objectList = sessionManager.objectList;

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


        private void GenerateLevel()
        {
            levelMatrix = new GameObject[levelWidth, levelHeight];

            for(int i = 0; i < levelHeight; i++)
            {
                for(int j = 0; j < levelWidth; j++)
                {
                    int randomSeed = Random.Range(0, 10);

                    if(i % 2 != 0  &&  j % 2 != 0)
                    {
                        levelMatrix[j, i] = objectList[0];                        
                    }
                    else if(randomSeed < randomness)
                    {
                        levelMatrix[j, i] = objectList[1];
                    }
                }
            }

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

            BuildLevel();
        }

        private void BuildLevel()
        {
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