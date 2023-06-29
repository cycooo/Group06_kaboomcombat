// MainMenu class
// ====================================================================================================================
// Handles the main submenu of the MainMenu (probably needs better names)


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;


namespace kaboomcombat
{
    public class MainMenu : MonoBehaviour
    {
        // Reference to the main menuController
        private MenuController menuController;

        // Reference to the root of this submenu
        public RectTransform panelMain;

        // Reference to the PlayerManager class
        public PlayerManager playerManager;

        // More references to ui elements
        public GameObject panelPlayerFrame;
        private GameObject panelPlayerHBox;

        // Reference to playerInputManager
        public PlayerInputManager playerInputManager;

        // References to UI element prefabs
        public GameObject panelPlayerPrefab;
        public GameObject panelJoinMessagePrefab;
        public GameObject panelJoinMessageSmallPrefab;
        public GameObject panelPlayerHboxPrefab;

        // Reference to text tooltip which displays status of lobby (ex. ready to play etc.)
        public TextMeshProUGUI textTooltip;

        // Reference to the playButton
        public Button buttonPlay;

        // Bomb icon to put next to selected button
        public Image imageSelector;

        private MenuActionAsset inputAsset;


        private void Awake()
        {
            // Assign references
            menuController = GetComponent<MenuController>();
            playerInputManager = GetComponent<PlayerInputManager>();
            playerManager = FindObjectOfType<PlayerManager>();

            // Create a new input asset
            inputAsset = new MenuActionAsset();

            // Call UpdateMenu to place UI elements accordingly
            UpdateMenu();
        }

        // Enable and disable joining on the playerInputManager when the script is enabled/disabled
        // This is so that players can't join the lobby when on another submenu
        private void OnEnable()
        {
            // Assign action
            inputAsset.Menu.Cancel.performed += QuitGame;
            inputAsset.Enable();
            playerInputManager.EnableJoining();
        }

        private void OnDisable()
        {
            inputAsset.Menu.Cancel.performed -= QuitGame;
            inputAsset.Disable();
            playerInputManager.DisableJoining();
        }

        private void QuitGame(InputAction.CallbackContext obj)
        {
            menuController.QuitGame();
        }


        // Function that gets called everytime a player object is instantiated by playerInputManager
        void OnPlayerJoined(PlayerInput playerInput)
        {
            // Store the player class
            Player player = playerInput.gameObject.GetComponent<Player>();

            // Assign values of player
            player.id = playerInput.playerIndex;
            player.inputDevice = playerInput.GetDevice<InputDevice>();
            player.controlScheme = playerInput.currentControlScheme;
            // Assign the playermodel according to the playerIndex (player id)
            player.playerModel = playerManager.playerModels[playerInput.playerIndex];

            // Add the player to the DataManager
            DataManager.playerListStatic.Add(player);

            // Update the menu to add the new player to the lobby
            UpdateMenu();
        }


        // Function that switches to the tutorial submenu
        public void SwitchToTutorial()
        {
            menuController.SwitchMenu(MenuState.TUTORIAL);
            menuController.soundSystem.PlaySound(Sounds.UI_SELECT);
        }


        // Function that switches to the options submenu
        public void SwitchToOptions()
        {
            menuController.SwitchMenu(MenuState.OPTIONS);
            menuController.soundSystem.PlaySound(Sounds.UI_SELECT);
        }


        // Function that moves the bomb menu pointer whenever the player moves their selections (ex. move from play
        // to tutorial or options)
        public void UpdateMenuPointer()
        {
            // Get the RectTransform of the currently selected Button
            RectTransform selectedButton = menuController.eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();

            // Place the pointer next to the selected Button
            imageSelector.rectTransform.anchoredPosition = new Vector2(selectedButton.anchoredPosition.x - 30f, selectedButton.anchoredPosition.y);
        }


        // Function that updates the menu UI
        public void UpdateMenu()
        {
            // Set the status message and play button color according to the amount of players in lobby
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

            // Destroy everything in the playerFrame
            foreach (Transform child in panelPlayerFrame.transform)
            {
                Destroy(child.gameObject);
            }

            // If there's no players, add the big join message to the playerFrame
            if (DataManager.playerListStatic.Count == 0)
            {
                Instantiate(panelJoinMessagePrefab, panelPlayerFrame.transform);
            }
            // If there are players, add a playerPanel for each one of them
            else
            {
                // Add the Horizontal Layout container
                panelPlayerHBox = Instantiate(panelPlayerHboxPrefab, panelPlayerFrame.transform);

                // Add the playerPanels
                for (int i = 0; i < DataManager.playerListStatic.Count; i++)
                {
                    // Set the string of the player panel
                    string playerString = "Player" + (i + 1);

                    // Instantiate the player panel and set it's parameters
                    GameObject panelPlayerInstance = Instantiate(panelPlayerPrefab, panelPlayerHBox.transform);
                    panelPlayerInstance.GetComponent<PanelPlayer>().playerId = i;
                    panelPlayerInstance.GetComponentInChildren<TextMeshProUGUI>().SetText(playerString);
                }

                // If the lobby is not full, add the small join message to the playerFrame after the playerPanels
                if (DataManager.playerListStatic.Count < 4)
                {
                    GameObject panelJoinMessageInstance = Instantiate(panelJoinMessageSmallPrefab, panelPlayerHBox.transform);
                    // Set it as the last sibling so it appears after everything else in the horizontal layout container
                    panelJoinMessageInstance.transform.SetAsLastSibling();
                }
            }
        }
    }
}
