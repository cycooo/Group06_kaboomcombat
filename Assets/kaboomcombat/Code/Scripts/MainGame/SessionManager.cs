using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kaboomcombat
{
    public class SessionManager : MonoBehaviour
    {
        public List<GameObject> objectList;


        void Start()
        {
            objectList = new List<GameObject>(); 
        }
    }
}
