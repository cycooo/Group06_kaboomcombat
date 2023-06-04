using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kaboomcombat
{
    public static class DataManager
    {
        public static List<Player> playerListStatic = new List<Player>();
        public static GameState gameState = GameState.WAITING;
    }
}