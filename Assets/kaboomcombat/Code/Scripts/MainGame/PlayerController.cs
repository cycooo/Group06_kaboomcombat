// PlayerController class
// ====================================================================================================================
// Handles the player's movement


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace kaboomcombat
{
    public class PlayerController : MonoBehaviour
    {
        // References
        private Player player;
        private SessionManager sessionManager;
        private List<GameObject> objectList;
        public GameObject playerModelContainer;
        public LayerMask collisionMask;

        // Input references
        private InputActionAsset inputAsset;
        private InputActionMap playerActionMap;
        private InputAction movement;
        private Vector2 inputDirection;

        // Movement fields
        public float moveTime = 0.2f;
        public float movementDeadzone = 0.5f;
        private bool isMoving = false;


        private GameObject bombInstance;


        private void Start()
        {
            // Get references to sessionManager and objectList
            player = GetComponent<Player>();
            sessionManager = player.sessionManager;
            objectList = sessionManager.objectList;
        }

        private void Awake()
        {
            // Get references to inputAsset and the "Player" action map
            inputAsset = GetComponent<PlayerInput>().actions;
            playerActionMap = inputAsset.FindActionMap("Player");
        }


        private void OnEnable()
       {
            // InputSystem components have to be enabled and disabled according to the object they are attached to.

            // Subscribe to the started event of the PlaceBomb action
            playerActionMap.FindAction("PlaceBomb").started += PlaceBomb;
            // Get reference to the movement action
            movement = playerActionMap.FindAction("Movement");
            // Enable the Player Action Map
            playerActionMap.Enable();        
       }


       private void OnDisable()
        {
            // Unsubscribe from the PlaceBomb action
            playerActionMap.FindAction("PlaceBomb").started -= PlaceBomb;
            // Disable the Player Action Map
            playerActionMap.Disable();
        }


        private void OnDestroy()
        {
            // Remove the player from the sessionManager playerList when it is destroyed
            sessionManager.playerList.Remove(gameObject);
            sessionManager.CheckGameOver();
            player.panelPlayerHud.ShowDeath();

            DataManager.leaderboard.Add(player.id);
            DataManager.playerKills[player.id] = player.kills;
        }


        private void FixedUpdate()
        {
            if(DataManager.gameState == GameState.PLAYING)
            {
                // Get the player's movement input vector
                inputDirection = movement.ReadValue<Vector2>();

                // Draw Rays to visualize player input
                Debug.DrawRay(transform.position, new Vector3(Mathf.RoundToInt(inputDirection.x), 0f, 0f), Color.blue);
                Debug.DrawRay(transform.position, new Vector3(0f, 0f, Mathf.RoundToInt(inputDirection.y)), Color.blue);

                // Check for input and if the player is currently moving
                if (inputDirection != Vector2.zero && !isMoving)
                {
                    // Fire two raycasts (horizontal and vertical directions) to check for objects in the path of the player
                    bool pathBlockedX = Physics.Raycast(transform.position, new Vector3(Mathf.RoundToInt(inputDirection.x), 0f, 0f), 1f, collisionMask);
                    bool pathBlockedY = Physics.Raycast(transform.position, new Vector3(0f, 0f, Mathf.RoundToInt(inputDirection.y)), 1f, collisionMask);

                    // Make the player face the direction they are trying to move in
                    // TODO: Make this less bad
                    if (inputDirection.x >= movementDeadzone)
                    {
                        transform.rotation = Quaternion.Euler(0f, -90f, 0f);
                    }
                    else if (inputDirection.x <= -movementDeadzone)
                    {
                        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                    }
                    else if (inputDirection.y >= movementDeadzone)
                    {
                        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    }
                    else if (inputDirection.y <= -movementDeadzone)
                    {
                        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    }


                    // Check for horizontal input over a certain threshold and if the player's path is blocked
                    if (Mathf.Abs(inputDirection.x) >= movementDeadzone && !pathBlockedX)
                    {
                        StartCoroutine(MovePlayer(new Vector3(Mathf.Sign(inputDirection.x), 0f, 0f)));

                    }
                    // Check for vertical input over a certain threshold and if the player's path is blocked
                    else if (Mathf.Abs(inputDirection.y) >= movementDeadzone && !pathBlockedY)
                    {
                        StartCoroutine(MovePlayer(new Vector3(0f, 0f, Mathf.Sign(inputDirection.y))));
                    }
                }
            }
        }


        // Coroutine to move the player
        private IEnumerator MovePlayer(Vector3 direction)
        {
            // Set the player to "moving" state
            isMoving = true;

            // Keep track of time passed
            float elapsedTime = 0f;

            // Get original position and use it to calculate target position
            Vector3 origPos = transform.position;
            Vector3 targetPos = origPos + direction;

            while (elapsedTime < moveTime)
            {
                // Linear interpolate the player's position until they reach the target position
                transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / moveTime);
                // Add to elapsed time
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // In case the player movement takes longer to complete than the time of the function, set the player's
            // position to the targetPos manually
            transform.position = targetPos;

            // Player is no longer moving
            isMoving = false;
        }


        // Function is called by the Input Event "PlaceBomb"
        private void PlaceBomb(InputAction.CallbackContext obj)
        {
            if(DataManager.gameState == GameState.PLAYING)
            {
                bool doReturn = false;

                if(!sessionManager.suddenDeathMode)
                {
                    if(bombInstance != null)
                    {
                        doReturn = true;
                    }
                }
                
                if(player.infiniBomb)
                {
                    doReturn = false;
                }

                if(doReturn) { return; }

                // Only place a bomb if a bomb is not already at the player's position
                if (LevelManager.SearchLevelTile(transform.position) == null)
                {
                    bombInstance = LevelManager.SpawnObject(objectList[2], transform.position);
                    bombInstance.GetComponent<BombController>().ownerPlayer = player;
                    bombInstance.GetComponent<BombController>().bombPower = player.bombPower;
                }
            }
        }
    }
}
