// PanelPlayerHud class
// ====================================================================================================================
// Class that handles the player information panels' behaviour. (This class needs a better name)


using TMPro;
using UnityEngine;


namespace kaboomcombat
{
    public class PanelPlayerHud : MonoBehaviour
    {
        // The id of the player assigned to the information panel
        public int playerId;
        private Player player;
        private SessionManager sessionManager;

        public TextMeshProUGUI textBombPower;
        public TextMeshProUGUI textKills;

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
                player = sessionManager.playerList[playerId].GetComponent<Player>();
            }
        }

        public void UpdatePanel()
        {
            CheckReferences();

            textBombPower.SetText((player.bombPower).ToString());
            textKills.SetText((player.kills).ToString());
        }
    }
}
