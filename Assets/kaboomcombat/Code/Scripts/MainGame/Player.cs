// Player Class
//=====================================================================================================================
// Handles Player parameters and references that are not needed for movement


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

        // Input parameters, used to determine what device and controlScheme each player is using
        public InputDevice inputDevice;
        public string controlScheme;

        // The playermodel parameter
        public GameObject playerModel;

        // Reference to the playerModelContainer which is used as a parent when instantiating the playermodel.
        public GameObject playerModelContainer;

    }
}
