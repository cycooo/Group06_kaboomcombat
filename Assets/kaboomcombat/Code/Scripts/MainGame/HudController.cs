using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace kaboomcombat
{
    public class HudController : MonoBehaviour
    {
        private SessionManager sessionManager;

        void Start()
        {
            sessionManager = FindObjectOfType<SessionManager>();
        }
    }
}
