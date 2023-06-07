// DataManager class
// ====================================================================================================================
// This static class is used to store global variables, such as the initial list of players and the gamestate enum
// Since it's static, the class can be accessed from anywhere without a reference (just use the DataManager type)


using System.Collections.Generic;


namespace kaboomcombat
{
    public static class DataManager
    {
        public static List<Player> playerListStatic = new List<Player>();
        public static GameState gameState = GameState.WAITING;
    }
}