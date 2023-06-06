// SessionManager class
// =====================================================================================================================


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace kaboomcombat
{
    public class SessionManager : MonoBehaviour
    {
        public float time = 180;

        public static SessionState sessionState;

        public List<GameObject> objectList = new List<GameObject>();
        public Transform[] playerSpawns = new Transform[4];

        public List<GameObject> playerList = new List<GameObject>();

        private PlayerInputManager playerInputManager;
        public HudController hudController;

        private bool debug = false;

        void Start()
        {
            hudController = FindObjectOfType<HudController>();
            playerInputManager = GetComponent<PlayerInputManager>();

            time++;


            if(DataManager.playerListStatic.Count == 0)
            {
                debug = true;
            }
            else
            {
                debug = false;
            }


            StartSession();
            SpawnPlayers();
        }


        private void FixedUpdate()
        {
            if(Mathf.FloorToInt(time) > 0)
            {
                UpdateTime();
            }
            else{
                GameOver(true);
            }
        }


        private void StartSession()
        {
            DataManager.gameState = GameState.PLAYING;
        }


        private void GameOver(bool timeOut)
        {

        }


        void OnPlayerJoined(PlayerInput playerInput)
        {
            playerInput.gameObject.transform.position = playerSpawns[playerInput.playerIndex].position;
            playerList.Add(playerInput.gameObject);
        }

        private void SpawnPlayers()
        {
            if(debug)
            {
                playerInputManager.JoinPlayer(-1, -1, "Keyboard");
                playerInputManager.JoinPlayer(-1, -1, "Gamepad");
            }
            else
            {
                foreach(Player player in DataManager.playerListStatic)
                {
                    playerInputManager.JoinPlayer(-1, -1, player.controlScheme, player.inputDevice);
                }
            }
        }


        private void UpdateTime()
        {
            time -= Time.deltaTime;

            hudController.UpdateTimer(time);
        }
    }
}
