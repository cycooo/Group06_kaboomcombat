// WallCrush class
// ====================================================================================================================
// Handles the falling crush walls


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
            // Destroy anything this object touches (i.e crush them)
            LevelManager.DestroyObject(other.gameObject);
        }


        // Coroutine that makes the block fall
        private IEnumerator AnimateWallCrush()
        {
            // Set the block high in the sky
            gameObject.transform.position = new Vector3(transform.position.x, 20f, transform.position.z);

            // Animate it falling
            LTDescr fallTween = LeanTween.move(gameObject, new Vector3(transform.position.x, 0f, transform.position.z), 0.5f);
            fallTween.setEaseInOutQuart();

            yield return null;
        }
    }
}
