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
            float time = Random.Range(2f, 2.5f);

            yield return new WaitForSeconds(time);

            LevelManager.SpawnObject(wallCrushPrefab, transform.position);

            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
}
