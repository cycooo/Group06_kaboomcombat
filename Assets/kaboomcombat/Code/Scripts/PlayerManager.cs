// PlayerManager class
// ====================================================================================================================
// Used to store constant information about the players (constant = doesn't change)
// Ex. list of playerModels, playerColors and playerPortraits for each id


using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public GameObject[] playerModels = new GameObject[4];
    public Color[] playerColors = new Color[4];
    public Sprite[] playerPortraits = new Sprite[4];

    private void Awake()
    {
        // Mark this object as DontDestroyOnLoad so that it is present in all scenes
        DontDestroyOnLoad(gameObject);
    }
}
