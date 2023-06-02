using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kaboomcombat
{
    public class LevelManager : MonoBehaviour
    {
        private int levelWidth = 13;
        private int levelHeight = 11;

        private int randomness = 7;

        // References
        private SessionManager sessionManager;

        private GameObject[,] levelMatrix;

        private List<GameObject> objectList;

        void Start()
        {
            sessionManager = GetComponent<SessionManager>();
            objectList = sessionManager.objectList;

            GenerateLevel(); 
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