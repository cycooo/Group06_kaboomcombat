using UnityEngine;
using UnityEngine.InputSystem;

namespace kaboomcombat
{
    public class Player : MonoBehaviour
    {
        public int id;
        public int kills;
        public GameObject playerModel;
        public InputDevice inputDevice;
        public string controlScheme;
    }
}
