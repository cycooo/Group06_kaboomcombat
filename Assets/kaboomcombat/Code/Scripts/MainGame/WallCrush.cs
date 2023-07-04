using System.Collections;
using UnityEngine;

namespace kaboomcombat
{
    public class WallCrush : MonoBehaviour
    {
        private void Awake()
        {
            StartCoroutine(AnimateWallCrush());
        }

        private void OnTriggerEnter(Collider other)
        {
            LevelManager.DestroyObject(other.gameObject);
        }

        private IEnumerator AnimateWallCrush()
        {
            gameObject.transform.position = new Vector3(transform.position.x, 20f, transform.position.z);

            LTDescr fallTween = LeanTween.move(gameObject, new Vector3(transform.position.x, 0f, transform.position.z), 0.5f);
            fallTween.setEaseInOutQuart();

            yield return null;
        }
    }
}
