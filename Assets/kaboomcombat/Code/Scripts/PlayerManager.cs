// PlayerManager class
// ====================================================================================================================
// Used to store constant information about the players (constant = doesn't change)
// Ex. list of playerModels, playerColors and playerPortraits for each id


using kaboomcombat;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public GameObject[] playerModels = new GameObject[4];
    public Color[] playerColors = new Color[4];
    public Sprite[] playerPortraits = new Sprite[4];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
