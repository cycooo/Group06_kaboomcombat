// SessionManager class
// =====================================================================================================================


using System.Collections.Generic;
using UnityEngine;


namespace kaboomcombat
{
    public class SessionManager : MonoBehaviour
    {
        public float time = 180;

        public List<GameObject> objectList;
        public HudController hudController;


        void Start()
        {
            // Create the objectList
            objectList = new List<GameObject>(); 
            hudController = FindObjectOfType<HudController>();

            time++;

            StartSession();
        }


        private void FixedUpdate()
        {
            if(Mathf.FloorToInt(time) > 0)
            {
                UpdateTime();
            }
            else{
                GameOver(true);
            }
        }


        private void StartSession()
        {
            DataManager.gameState = GameState.PLAYING;
        }


        private void GameOver(bool timeOut)
        {

        }


        private void UpdateTime()
        {
            time -= Time.deltaTime;

            hudController.UpdateTimer(time);
        }
    }
}
