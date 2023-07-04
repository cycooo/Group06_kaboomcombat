// SessionManager class
// ====================================================================================================================
// Handles the parameters and logic of the main game (Ex. time, players alive, spawning players, changing game state etc.)


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace kaboomcombat
{
    public class SessionManager : MonoBehaviour
    {
        // Game parameters
        public float time = 150;
        public int bombPowerMax = 9;

        public bool fastMode = false;

        // Powerup parameters
        public float powerupTimer = 0f;
        public int powerupCounter = 0;
        public int powerupMax = 3;

        // List containing every object that is placed in the levelMatrix
        public List<GameObject> objectList = new List<GameObject>();
        // List containing every powerup object
        public List<GameObject> powerupList = new List<GameObject>();
        // Array containing each player's spawnpoint
        public Transform[] playerSpawns = new Transform[4];
        // List which is used to store and keep track of players currently in the game
        public List<GameObject> playerList = new List<GameObject>();

        // References
        private PlayerInputManager playerInputManager;
        public HudController hudController;
        private CameraController cameraController;


        void Awake()
        {
            // Assign references
            hudController = GetComponent<HudController>();
            playerInputManager = GetComponent<PlayerInputManager>();
            cameraController = FindObjectOfType<CameraController>();

            // Add one second to the time because it skips the first second when displayed in game
            time++;
        }

        private void Start()
        {
            Physics.gravity = new Vector3(0f, -9.8f, 0f);

            // Start the session(Not Game!) and spawn the players
            StartSession();
            SpawnPlayers();
        }


        private void FixedUpdate()
        {
            if (DataManager.gameState == GameState.PLAYING)
            {
                UpdateTime();
                
                
                UpdatePowerUpTimer();
            }
        }

        // Function that starts the session
        // TODO: Implement round start countdown
        private void StartSession()
        {
            // Set the gamestate to waiting to the players can't move yet
            DataManager.gameState = GameState.WAITING;

            // Stop any music track that may be playing
            SoundSystem.instance.StopMusic();

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

            SoundSystem.instance.PlayMusic(Music.JAZZ_ACTION);
        }


        // TODO: Function that handles game over scenarios
        private void GameOver(bool timeOut)
        {
            DataManager.gameState = GameState.GAMEOVER;
            
            foreach(GameObject player in playerList)
            {
                player.GetComponent<Player>().god = true;
            }

            if (!timeOut)
            {
                hudController.CloseHud();
                cameraController.MoveTo(playerList[0].transform.position, 1f);
                cameraController.ZoomTo(10f, 1f);
            }
            else
            {
                // TODO
            }
        }


        // Function that checks if the game is over (if only 1 player remains)
        public void CheckGameOver()
        {
            if(playerList.Count == 1)
            {
                GameOver(false);
            }
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
                player.id = DataManager.playerListStatic[playerInput.playerIndex].id;
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


        // Function to handle powerup spawning, which works on a timer
        private void UpdatePowerUpTimer()
        {
            // If the timer reaches 0, try spawning a powerup
            if(powerupTimer <= 0)
            {
                // Get a random position in the level
                Vector3 position = LevelManager.GetRandomMatrixPosition();
                // If the random position is not occupied by another object, continue
                if (LevelManager.SearchLevelTile(position) == null)
                {
                    // Choose a random powerup from powerupList
                    int randomObjectIndex = Random.Range(0, powerupList.Count);
                    // Do not spawn a new powerup if there are already 4 powerups in the level
                    if (powerupCounter < powerupMax)
                    {
                        // Spawn the powerup and add 1 to the powerupCounter to keep track of them
                        LevelManager.SpawnObject(powerupList[randomObjectIndex], position);
                        powerupCounter++;
                    }
                }
                // Choose a new random timer until the next spawning attempt
                powerupTimer = Random.Range(2f, 5f);
            }
            
            powerupTimer -= Time.deltaTime;
        }


        // Update the time
        private void UpdateTime()
        {
            // Update the time every frame, unless the time is 0
            if (Mathf.FloorToInt(time) > 0)
            {
                    time -= Time.deltaTime;
            }
            else
            {
                // Set time to 0 in case it goes negative before we stop counting
                time = 0f;
                // Call GameOver with a timeOut value of true to indicate that the time has run out
                GameOver(true);
            }

            if (Mathf.FloorToInt(time) == 60 && !fastMode)
            {
                fastMode = true;

                StartCoroutine(hudController.ShowMessageHurryUp());
                SoundSystem.instance.StopMusic();
                SoundSystem.instance.PlayMusic(Music.JAZZ_ACTION_FAST);
            }

            // Update the timer hud element
            hudController.UpdateTimer(time);
            
        }
    }
}
