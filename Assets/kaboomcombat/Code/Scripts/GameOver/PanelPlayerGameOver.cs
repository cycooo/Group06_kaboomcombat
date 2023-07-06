// PanelPlayerGameOver class
// ====================================================================================================================
// Clone of PanelPlayerHud adjusted for the GameOver scene


using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace kaboomcombat
{
    public class PanelPlayerGameOver : MonoBehaviour
    {
        // The id of the player assigned to the information panel
        public int playerId;
        private Player player;
        private SessionManager sessionManager;

        public RectTransform panelPlayerStats;
        public RectTransform playerPortrait;

        public Image imageSkull;
        public Image imagePlayerProfile;
        public Image imagePlayerProfileOutline;
        public Image dropShadow;
        public Image imagePlayermodel;

        public TextMeshProUGUI textKills;

        private string kills;


        private void Start()
        {
            // Assign the player portrait class's playerId, since it is a child of this object
            GetComponentInChildren<PlayerPortrait>().playerId = playerId;
            CheckReferences();
        }

        
        // Function that checks if references are null and assigns them if they are
        private void CheckReferences()
        {
            if(sessionManager == null)
            {
                sessionManager = FindObjectOfType<SessionManager>();
            }
            if(player == null)
            {
                kills = DataManager.playerKills[playerId].ToString();
            }
        }


        // Function to update the panel
        public void UpdatePanel()
        {
            CheckReferences();

             
            textKills.SetText((kills).ToString());
        }

    }
}
