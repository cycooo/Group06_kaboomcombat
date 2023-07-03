// PanelPlayer class
// ====================================================================================================================
// Handles the player panels used in the main menu's lobby UI


using UnityEngine;


namespace kaboomcombat
{
    public class PanelPlayer : MonoBehaviour
    {
        // the id of the player we wish to assign to this player panel
        public int playerId;

        private void Start()
        {
            // set the player portrait's id, since it's a child of this object
            GetComponentInChildren<PlayerPortrait>().playerId = playerId;
        }
    }
}
