// Player Class
//=====================================================================================================================
// Handles Player parameters and references that are not needed for movement


using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace kaboomcombat
{
    public class Player : MonoBehaviour
    {
        // Player parameters
        public int id;
        public int kills = 0;
        public int bombPower = 2;
        public bool god = false;
        public bool fast = false;

        // Input parameters, used to determine what device and controlScheme each player is using
        public InputDevice inputDevice;
        public string controlScheme;

        // The playermodel parameter
        public GameObject playerModel;

        // Effect prefabs
        public GameObject effectMoveSpeed;
        public GameObject effectGod;

        // Reference to the playerModelContainer which is used as a parent when instantiating the playermodel.
        public GameObject playerModelContainer;

        // References
        public PlayerController playerController;
        public SessionManager sessionManager;
        public PanelPlayerHud panelPlayerHud;


        private void Awake()
        {
            if (DataManager.gameState != GameState.MENU)
            {
                playerController = GetComponent<PlayerController>();
                sessionManager = FindObjectOfType<SessionManager>();
                panelPlayerHud = sessionManager.hudController.panelPlayerHudArray[id].GetComponent<PanelPlayerHud>();
            }
        }


        // Function that kills the player if they are not invulnerable
        public void Kill()
        {
            if(!god)
            {
                Destroy(gameObject);
            }
        }


        // Wrapper function to activate the god powerup
        public void SetPowerUpGod()
        {
            StartCoroutine(SetGodForSeconds(10f));
        }


        // Wrapper function to actibvate the movespeed powerup
        public void SetPowerUpMoveSpeed()
        {
            StartCoroutine(playerController.SetMoveTimeForSeconds(0.12f, 10f));
        }


        // Coroutine to activate the god powerup and disable it after duration
        public IEnumerator SetGodForSeconds(float duration)
        {
            if(!god)
            {
                god = true;
                GameObject effectGodInstance = Instantiate(effectGod, gameObject.transform);

                yield return new WaitForSeconds(duration);

                god = false;
                Destroy(effectGodInstance);
            }
        }


        // Function that increments kills by 1
        public void IncrementKills()
        {
            kills++;
            panelPlayerHud.UpdatePanel();
        }


        // Function that increments bombPower by 1
        public void IncrementBombPower()
        {
            // Check if bombPower is lower than the Max defined in SessionManager
            if(bombPower < sessionManager.bombPowerMax)
            {
                // Increment bombPower
                bombPower++;
                panelPlayerHud.UpdatePanel();
            }
        }
    }
}
