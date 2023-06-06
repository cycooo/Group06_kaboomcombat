using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

namespace kaboomcombat
{
    public class MainMenu : MonoBehaviour
    {
        private MenuController menuController;


        public RectTransform panelMain;

        public GameObject panelPlayerFrame;
        private GameObject panelPlayerHBox;

        private PlayerInputManager playerInputManager;


        private Color[] playerColors = new Color[4];


        public GameObject panelPlayerPrefab;
        public GameObject panelJoinMessagePrefab;
        public GameObject panelJoinMessageSmallPrefab;
        public GameObject panelPlayerHboxPrefab;


        public TextMeshProUGUI textTooltip;

        public Button buttonPlay;

        // Bomb icon to put next to selected button
        public Image imageSelector;


        private void Awake()
        {
            menuController = GetComponent<MenuController>();
            playerInputManager = GetComponent<PlayerInputManager>();

            playerColors[0] = Color.HSVToRGB(219, 73, 80);
            playerColors[1] = Color.HSVToRGB(4, 71, 93);
            playerColors[2] = Color.HSVToRGB(47, 74, 93);
            playerColors[3] = Color.HSVToRGB(113, 62, 82);

            UpdateMenu();
        }

        private void OnEnable()
        {
            playerInputManager.EnableJoining();
        }

        private void OnDisable()
        {
            playerInputManager.DisableJoining();
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


        public void SwitchToTutorial()
        {
            menuController.SwitchMenu(MenuState.TUTORIAL);
        }

        public void SwitchToOptions()
        {
            menuController.SwitchMenu(MenuState.OPTIONS);
        }


        public void UpdateMenuPointer()
        {
            RectTransform selectedButton = menuController.eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();

            imageSelector.rectTransform.anchoredPosition = new Vector2(selectedButton.anchoredPosition.x - 30f, selectedButton.anchoredPosition.y);
        }


        public void UpdateMenu()
        {
            if (DataManager.playerListStatic.Count < 2)
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
                    Color playerColor = playerColors[i];


                    GameObject panelPlayerInstance = Instantiate(panelPlayerPrefab, panelPlayerHBox.transform);
                    panelPlayerInstance.GetComponentInChildren<TextMeshProUGUI>().SetText(playerString);
                }

                if (DataManager.playerListStatic.Count < 4)
                {
                    GameObject panelJoinMessageInstance = Instantiate(panelJoinMessageSmallPrefab, panelPlayerHBox.transform);
                    panelJoinMessageInstance.transform.SetAsLastSibling();
                }
            }
        }
    }
}
