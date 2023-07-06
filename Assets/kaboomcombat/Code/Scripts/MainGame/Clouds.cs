using UnityEngine;

public class Clouds : MonoBehaviour
{
    public float timeToRotate = 480f;

    void Start()
    {
        LeanTween.rotateAround(gameObject, Vector3.up, 360f , timeToRotate);
    }
}
