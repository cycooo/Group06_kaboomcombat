using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.Build.Player;

namespace kaboomcombat
{
    public class MainMenuController : MonoBehaviour
    {
        private float spawnInterval = 1f;
        private Vector2 xBounds = new Vector2(-10f, 10f);
        private Vector2 zBounds = new Vector2(-12f, 16f);

        public GameObject[] spawnLimits = new GameObject[2];

        // List of prefabs to instantiate for the background
        public List<GameObject> bombList = new List<GameObject>();

        public GameObject panelPlayerFrame;
        private GameObject panelPlayerHBox;


        private Color player1Color = Color.HSVToRGB(219, 73, 80);
        private Color player2Color = Color.HSVToRGB(4, 71, 93);
        private Color player3Color = Color.HSVToRGB(47, 74, 93);
        private Color player4Color = Color.HSVToRGB(113, 62, 82);


        public GameObject panelPlayerPrefab;
        public GameObject panelJoinMessagePrefab;
        public GameObject panelJoinMessageSmallPrefab;
        public GameObject panelPlayerHboxPrefab;
        

        public TextMeshProUGUI textTooltip;

        public Button buttonPlay;

        // Bomb icon to put next to selected button
        public Image imageSelector;

        private EventSystem eventSystem;
        

        void Awake()
        {
            DataManager.gameState = GameState.MENU;

            eventSystem = FindObjectOfType<EventSystem>();

            Physics.gravity = new Vector3(0f, -1f, 0f);

            AssignSpawnLimits();
            SpawnBackgroundObject();

            UpdateMenu();
        }

        void OnPlayerJoined(PlayerInput playerInput)
        {
            Player player = playerInput.gameObject.GetComponent<Player>();
            
            player.id = playerInput.playerIndex;
            player.inputDevice = playerInput.devices[0];
            player.controlScheme = playerInput.currentControlScheme;
            

            DataManager.playerListStatic.Add(player);

            UpdateMenu();
        }


        public void StartGame()
        {
            if(DataManager.playerListStatic.Count >= 2)
            {
                SceneManager.LoadScene("MainGame");
            }
        }


        private void UpdateMenu()
        {
            if(DataManager.playerListStatic.Count < 2)
            {
                textTooltip.SetText("You need at least 2 players to play.");

                var tempColor = buttonPlay.image.color;
                tempColor.a = 0.4f;
                buttonPlay.image.color = tempColor;
            }
            else
            {
                textTooltip.SetText("Ready to play.");

                var tempColor = buttonPlay.image.color;
                tempColor.a = 1f;
                buttonPlay.image.color = tempColor;
            }


            foreach (Transform child in panelPlayerFrame.transform)
            {
                Destroy(child.gameObject);
            }

            if (DataManager.playerListStatic.Count == 0)
            {
                Instantiate(panelJoinMessagePrefab, panelPlayerFrame.transform);
            }
            else
            {
                panelPlayerHBox = Instantiate(panelPlayerHboxPrefab, panelPlayerFrame.transform);

                for (int i = 0; i < DataManager.playerListStatic.Count; i++)
                {
                    string playerString = "Player";
                    Color playerColor = player1Color;


                    switch(i)
                    {
                        case 0:
                            playerString = "Player 1";
                            playerColor = player1Color;

                            break;
                        case 1:
                            playerString = "Player 2";
                            playerColor = player2Color;

                            break;
                        case 2:
                            playerString = "Player 3";
                            playerColor = player3Color;

                            break;
                        case 3:
                            playerString = "Player 4";
                            playerColor = player4Color;

                            break;
                    }


                    GameObject panelPlayerInstance = Instantiate(panelPlayerPrefab, panelPlayerHBox.transform);
                    panelPlayerInstance.GetComponentInChildren<TextMeshProUGUI>().SetText(playerString);
                }

                if(DataManager.playerListStatic.Count < 4)
                {
                    GameObject panelJoinMessageInstance = Instantiate(panelJoinMessageSmallPrefab, panelPlayerHBox.transform);
                    panelJoinMessageInstance.transform.SetAsLastSibling();
                }
            }
        }


        public void UpdateMenuPointer()
        {
            RectTransform selectedButton = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();

            imageSelector.rectTransform.anchoredPosition = new Vector2(selectedButton.anchoredPosition.x - 30f, selectedButton.anchoredPosition.y); 
        }


        private void AssignSpawnLimits()
        {
            xBounds = new Vector2(spawnLimits[0].transform.position.x, spawnLimits[1].transform.position.x);
            zBounds = new Vector2(spawnLimits[0].transform.position.z, spawnLimits[1].transform.position.z);
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
