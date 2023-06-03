using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace kaboomcombat
{
    public class HudController : MonoBehaviour
    {
        private SessionManager sessionManager;
        public TextMeshProUGUI textTimer;

        void Start()
        {
            sessionManager = FindObjectOfType<SessionManager>();
        }


        public void UpdateTimer(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);

            textTimer.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
    }
}
