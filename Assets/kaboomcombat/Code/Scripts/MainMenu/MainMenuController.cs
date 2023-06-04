using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace kaboomcombat
{
    public class MainMenuController : MonoBehaviour
    {
        private float spawnInterval = 1f;
        private Vector2 xBounds = new Vector2(-10f, 10f);
        private Vector2 zBounds = new Vector2(-12f, 16f);


        public List<GameObject> bombList = new List<GameObject>();

        public Image imageSelector;

        private EventSystem eventSystem;
        

        void Awake()
        {
            eventSystem = FindObjectOfType<EventSystem>();

            Physics.gravity = new Vector3(0f, -1f, 0f);
            Debug.Log(Physics.gravity);
            SpawnBackgroundObject();
        }


        public void UpdateMenuPointer()
        {
            RectTransform selectedButton = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();

            imageSelector.rectTransform.anchoredPosition = new Vector2(selectedButton.anchoredPosition.x - 30f, selectedButton.anchoredPosition.y); 
        }


        private void SpawnBackgroundObject()
        {
            float xPos = Random.Range(xBounds.x, xBounds.y);
            float zPos = Random.Range(zBounds.x, zBounds.y);

            float torqueValue = 30f;
            Vector3 torque = new Vector3(Random.Range(-torqueValue, torqueValue), Random.Range(-torqueValue, torqueValue), Random.Range(-torqueValue, torqueValue));

            GameObject bombInstance = Instantiate(bombList[Random.Range(0, 3)], new Vector3(xPos, 12f, zPos), Random.rotation);
            bombInstance.GetComponent<Rigidbody>().drag = Random.Range(0f, 2f);
            bombInstance.GetComponent<Rigidbody>().AddTorque(torque);

            Invoke("SpawnBackgroundObject", spawnInterval);
        }
    }
}
