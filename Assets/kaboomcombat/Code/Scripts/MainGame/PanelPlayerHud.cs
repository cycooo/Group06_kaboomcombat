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
            sessionManager = FindObjectOfType<SessionManager>();
        }

        public void UpdatePanel()
        {
            player = sessionManager.playerList[playerId].GetComponent<Player>();

            textBombPower.SetText((player.bombPower).ToString());
            textKills.SetText((player.kills).ToString());
        }
    }
}
