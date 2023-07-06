// CrushIndicator class
// ====================================================================================================================
// Handles the flashing red indicator before a crush wall appears


using System.Collections;
using UnityEngine;


namespace kaboomcombat
{
    public class CrushIndicator : MonoBehaviour
    {

        public GameObject wallCrushPrefab;

        void Start()
        {
            StartCoroutine(SpawnWallCrush());
        }

        private IEnumerator SpawnWallCrush()
        {
            // Get a random time to add variation to falling blocks
            float time = Random.Range(2f, 2.5f);

            yield return new WaitForSeconds(time);

            // Spawn the falling block
            LevelManager.SpawnObject(wallCrushPrefab, transform.position);

            // Destroy the indicator when the block lands
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
}
