using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kaboomcombat
{
    public class Powerup : MonoBehaviour
    {
        private void OnDestroy()
        {
            FindObjectOfType<SessionManager>().powerupCounter--;
            LevelManager.DestroyObject(gameObject);
        }
    }
}
