// PlayerPortrait Class
// ====================================================================================================================
// Mini-class that assigns values needed for the player's portraits


using UnityEngine;
using UnityEngine.UI;


namespace kaboomcombat
{
    public class PlayerPortrait : MonoBehaviour
    {
        // Reference to the playerManager, which is where all constant information about players is stored
        // (ex. colors for each player, their portrait picture and their playermodel
        // NOTE: PlayerManager is present in every scene, so we can use FindObjectOfType anywhere
        public PlayerManager playerManager;

        // References to images
        public Image imagePlayerProfile;
        public Image imagePlayermodel;

        // The player's id we wish to assign this panel to
        public int playerId;

        private void Start()
        {
            // Get reference to playermanager;
            playerManager = FindObjectOfType<PlayerManager>();

            UpdatePortrait();

            /*
            // Set the color and portrait picture of the player
            imagePlayerProfile.color = playerManager.playerColors[playerId];
            imagePlayermodel.sprite = playerManager.playerPortraits[playerId];
            */
        }

        public void UpdatePortrait()
        {
            imagePlayerProfile.color = playerManager.playerColors[playerId];
            imagePlayermodel.sprite = playerManager.playerPortraits[playerId];
        }
    }
}
