// SessionManager class
// =====================================================================================================================
// Handles the parameters and logic of the main game (Ex. time, players alive, spawning players, changing game state etc.)


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace kaboomcombat
{
    public class SessionManager : MonoBehaviour
    {
        // Game parameters
        public float time = 180;

        // List containing every object that is placed in the levelMatrix
        public List<GameObject> objectList = new List<GameObject>();
        // Array containing each player's spawnpoint
        public Transform[] playerSpawns = new Transform[4];
        // List which is used to store and keep track of players currently in the game
        public List<GameObject> playerList = new List<GameObject>();

        // References
        private PlayerInputManager playerInputManager;
        private HudController hudController;


        void Awake()
        {
            // Assign references
            hudController = GetComponent<HudController>();
            playerInputManager = GetComponent<PlayerInputManager>();

            // Add one second to the time because it skips the first second when displayed in game
            time++;

            // Start the session(Not Game!) and spawn the players
            StartSession();
            SpawnPlayers();
        }


        private void FixedUpdate()
        {
            // Update the time every frame, unless the time is 0
            if(Mathf.FloorToInt(time) > 0)
            {
                UpdateTime();
            }
            else{
                // Call GameOver with a timeOut value of true to indicate that the time has run out
                GameOver(true);
            }
        }

        // Function that starts the session
        // TODO: Implement round start countdown
        private void StartSession()
        {
            // Set the gamestate to waiting to the players can't move yet
            DataManager.gameState = GameState.WAITING;
            // Start the Countdown animation
            StartCoroutine(hudController.StartHudCountdown());
        }

        // Function that starts the gameplay
        public void StartGame()
        {
            // Set the gamestate so players can move
            DataManager.gameState = GameState.PLAYING;
            hudController.panelHud.SetActive(true);
            hudController.OpenHud();
        }


        // TODO: Function that handles game over scenarios
        private void GameOver(bool timeOut)
        {

        }


        // Function that gets called each time a player is instantiated via the playerInputManager
        // with JoinPlayer()
        void OnPlayerJoined(PlayerInput playerInput)
        {
            // Get a reference to the Player class
            Player player = playerInput.gameObject.GetComponent<Player>();

            // Assign values
            try
            {
                player.playerModel = DataManager.playerListStatic[playerInput.playerIndex].playerModel;
                player.controlScheme = DataManager.playerListStatic[playerInput.playerIndex].controlScheme;
                player.inputDevice = DataManager.playerListStatic[playerInput.playerIndex].inputDevice;
            }
            catch
            {
                Debug.Log("Player does not exist in DataManager");
            }
            
            // Set the playermodel
            if(player.playerModel != null)
            {
                // Delete the old playermodel
                foreach (Transform child in player.playerModelContainer.transform)
                {
                    Destroy(child.gameObject);
                }

                // Add the new playermodel with playerModelContainer as it's parent
                Instantiate(player.playerModel, player.playerModelContainer.transform);
            }

            // Set the player's position according to it's id
            // (0 spawns top left, 1 bottom right, 2 top right, 3 bottom left)
            player.transform.position = playerSpawns[playerInput.playerIndex].position;

            // Add the player gameObject to the playerList
            playerList.Add(playerInput.gameObject);
        }


        // Function that spawns players according to the player list in DataManager
        private void SpawnPlayers()
        {
            foreach (Player player in DataManager.playerListStatic)
            {
                // JoinPlayer and assign the correct controlScheme and inputDevice
                playerInputManager.JoinPlayer(-1, -1, player.controlScheme, player.inputDevice);
            }
        }


        // Update the time
        private void UpdateTime()
        {
            time -= Time.deltaTime;

            // Update the timer hud element
            hudController.UpdateTimer(time);
        }
    }
}
