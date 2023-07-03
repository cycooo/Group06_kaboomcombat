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

        // Ragdoll prefab
        public GameObject playerRagdoll;

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
                GameObject playerRagdollInstance = Instantiate(playerRagdoll, transform.position, playerRagdoll.transform.rotation);
                playerRagdollInstance.GetComponent<PlayerRagdoll>().AddPlayermodel(playerModel);
                Destroy(gameObject);

            }
        }


        public void SetPowerupGod(float duration)
        {
            StartCoroutine(SetGodForSeconds(10f));
        }


        public void SetPowerupMoveSpeed(float moveTime, float duration)
        {
            StartCoroutine(SetMoveTimeForSeconds(0.12f, 10f));
        }


        public IEnumerator SetGodForSeconds(float duration)
        {
            if (!god)
            {
                god = true;
                GameObject effectGodInstance = Instantiate(effectGod, gameObject.transform);

                yield return new WaitForSeconds(duration);

                god = false;
                Destroy(effectGodInstance);
            }
        }

        public IEnumerator SetMoveTimeForSeconds(float moveTime, float duration)
        {
            if (!fast)
            {
                fast = true;

                float moveTimeOrig = playerController.moveTime;
                playerController.moveTime = moveTime;
                GameObject effectMoveSpeedInstance = Instantiate(effectMoveSpeed, gameObject.transform);

                yield return new WaitForSeconds(duration);

                playerController.moveTime = moveTimeOrig;
                Destroy(effectMoveSpeedInstance);

                fast = false;
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
