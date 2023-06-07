// PanelPlayerHud class
// ====================================================================================================================
// Class that handles the player information panels' behaviour. (This class needs a better name)


using UnityEngine;


namespace kaboomcombat
{
    public class PanelPlayerHud : MonoBehaviour
    {
        // The id of the player assigned to the information panel
        public int playerId;

        void Start()
        {
            // Assign the player portrait class's playerId, since it is a child of this object
            GetComponentInChildren<PlayerPortrait>().playerId = playerId;
        }
    }
}
