using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kaboomcombat
{
    public class BombController : MonoBehaviour
    {
        void Start()
        {
            Invoke("Kill", 3);
        }


        private void Kill()
        {
            LevelManager.DestroyObject(gameObject);
        }
    }
}
