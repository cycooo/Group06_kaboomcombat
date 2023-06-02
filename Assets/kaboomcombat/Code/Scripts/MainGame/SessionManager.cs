// SessionManager class
// =====================================================================================================================


using System.Collections.Generic;
using UnityEngine;


namespace kaboomcombat
{
    public class SessionManager : MonoBehaviour
    {
        public List<GameObject> objectList;


        void Start()
        {
            // Create the objectList
            objectList = new List<GameObject>(); 
        }
    }
}
