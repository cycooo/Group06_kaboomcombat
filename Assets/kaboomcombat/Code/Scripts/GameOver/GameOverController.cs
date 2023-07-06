// GameOverController Class
// ====================================================================================================================
// Handles the game over scene


using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


namespace kaboomcombat{
    public class GameOverController : MonoBehaviour
    {
        // Arrays of place texts and playerPanels
        public TMP_Text[] placeTexts;
        public GameObject[] panelPlayerHudArray = new GameObject[4];

        // Reference to the inputasset 
        private MenuActionAsset inputAsset;


        public void Awake(){
            // Set all player panels to inactive
            for(int i = 0; i < panelPlayerHudArray.Length;i++){
                panelPlayerHudArray[i].SetActive(false);
            }

            // Create a new input asset
            inputAsset = new MenuActionAsset();

            InitializeHud();
            TMPhandler();
        }


        private void OnEnable()
        {
            // Assign actions
            inputAsset.Menu.Cancel.performed += QuitGame;
            inputAsset.Menu.Submit.performed += SwitchToMainMenu;
            inputAsset.Enable();
        }

        private void OnDisable()
        {
            // Unassign actions
            inputAsset.Menu.Cancel.performed -= QuitGame;
            inputAsset.Menu.Submit.performed -= SwitchToMainMenu;
            inputAsset.Disable();
        }


        // Self explanatory
        private void QuitGame(InputAction.CallbackContext obj)
        {
            Application.Quit();
        }


        private void SwitchToMainMenu(InputAction.CallbackContext obj)
        {
            SceneManager.LoadScene("MainMenu");
        }


        // Show the right numbers of texts, according to the number of players
        public void TMPhandler()
        {
            int total = DataManager.leaderboard.Count;

            for (int i = 0; i < placeTexts.Length; i++)
            {
                if (i < total)
                {
                    //placeTexts[i].text = "Player " + DataManager.leaderboard[total - i - 1].ToString();       
                }
                else
                {
                    placeTexts[i].gameObject.SetActive(false);
                }
            }
        }


        public void InitializeHud()
        {
            // Players are added to the leaderboard when they die, so to get a ranking based on who lived the longest,
            // we have to reverse the list

            DataManager.leaderboard.Reverse();

            // Assign the player panels according to the reversed list
            for(int i = 0; i < DataManager.leaderboard.Count; i++)
            {
                panelPlayerHudArray[i].GetComponent<PanelPlayerGameOver>().playerId = DataManager.leaderboard[i];
                panelPlayerHudArray[i].SetActive(true);
            }

            // Update the panels to actually display the new information
            UpdatePlayerPanels();
        }


        // Function to update all player panels at once
        public void UpdatePlayerPanels()
        {
            // For each panel in panelPlayerHudArray, call the UpdatePanel function
            for(int i = 0; i < panelPlayerHudArray.Length; i++)
            {
                panelPlayerHudArray[i].GetComponent<PanelPlayerGameOver>().UpdatePanel();
            }
        }
    }
}
