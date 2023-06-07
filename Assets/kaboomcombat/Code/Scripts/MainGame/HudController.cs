// HudController Class
// ====================================================================================================================
// Handles the in-game hud (timer, player information panels etc.)


using UnityEngine;
using TMPro;


namespace kaboomcombat
{
    public class HudController : MonoBehaviour
    {
        // References
        private SessionManager sessionManager;
        public TextMeshProUGUI textTimer;
        // Reference to the array used to store all player information panels for easy access
        public GameObject[] panelPlayerHudArray = new GameObject[4];


        void Awake()
        {
            // Assign the sessionManager
            sessionManager = FindObjectOfType<SessionManager>();

            // Initialize the hud ._.
            InitializeHud();
        }

        
        // Function used to update the timer each frame
        public void UpdateTimer(float time)
        {
            // Calculate minutes and seconds from the total time float
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);

            // Modify the timer's text with the current time remaining
            textTimer.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }


        // Function that initializes the hud.
        // Determine the amount of player information panels to display, what to display on them.
        // This function is called when the level first starts, otherwise we get errors from the information panels
        // because variables weren't assigned
        public void InitializeHud()
        {
            // First, disable all player information panels to avoid errors on level load
            foreach(GameObject playerPanelHud in panelPlayerHudArray)
            {
                playerPanelHud.SetActive(false);
            }

            // Find out how many player information panels we need, set the id for each one and enable only the ones
            // that are needed (Ex. With 2 players, only 2 information panels are enabled)
            // We use DataManager's playerListStatic count to find out the total number of players
            for(int i = 0; i < DataManager.playerListStatic.Count; i++)
            {
                panelPlayerHudArray[i].GetComponent<PanelPlayerHud>().playerId = i;
                panelPlayerHudArray[i].SetActive(true);
            }
        }
    }
}
