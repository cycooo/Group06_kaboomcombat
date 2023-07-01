// PanelPlayerHud class
// ====================================================================================================================
// Class that handles the player information panels' behaviour. (This class needs a better name)


using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace kaboomcombat
{
    public class PanelPlayerHud : MonoBehaviour
    {
        // The id of the player assigned to the information panel
        public int playerId;
        private Player player;
        private SessionManager sessionManager;

        public RectTransform panelPlayerStats;
        public RectTransform playerPortrait;

        public Image imageSkull;
        public Image imagePlayerProfile;
        public Image imagePlayerProfileOutline;
        public Image dropShadow;
        public Image imagePlayermodel;

        public TextMeshProUGUI textBombPower;
        public TextMeshProUGUI textKills;


        private void Awake()
        {
            // Assign the player portrait class's playerId, since it is a child of this object
            PlayerPortrait playerPortrait = GetComponentInChildren<PlayerPortrait>();

            Debug.Log("[PanelPlayerHud][Start] playerPortrait=" + playerPortrait);
            Debug.Log("[PanelPlayerHud][Start] playerId=" + playerId);

            playerPortrait.playerId = playerId;

            CheckReferences();
        }

        
        // Function that checks if references are null and assigns them if they are
        private void CheckReferences()
        {
            if(sessionManager == null)
            {
                sessionManager = FindObjectOfType<SessionManager>();
            }
            if(player == null)
            {
                player = sessionManager.playerList[playerId].GetComponent<Player>();
            }

            Debug.Log("[PanelPlayerHud][CheckReferences] sessionManager=" + sessionManager + ", player=" + player);
        }

        public void UpdatePanel()
        {
            CheckReferences();

            textBombPower.SetText((player.bombPower).ToString());
            textKills.SetText((player.kills).ToString());

            Debug.Log("[PanelPlayerHud][UpdatePanel] panelPlayerHud[" + playerId + "] updated");
        }

        
        // Animate the panel when the player dies
        public void ShowDeath()
        {
            // Set the skull at a scale of zero and flipped upside down at the start
            imageSkull.gameObject.SetActive(true);
            imageSkull.rectTransform.localScale = Vector3.zero;
            imageSkull.rectTransform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));

            // Animate the different elements into place
            LeanTween.scale(playerPortrait, new Vector3(0.4f, 0.4f, 1f), 0.3f).setEaseOutBounce();
            LeanTween.scale(imageSkull.rectTransform, new Vector3(1f, 1f, 1f), 0.5f).setEaseOutBounce();
            LeanTween.rotateAround(imageSkull.rectTransform, Vector3.forward, 180f, 0.3f);
            LeanTween.alpha(imagePlayermodel.rectTransform, 0f, 0.3f).setOnComplete(delegate ()
            {
                imagePlayermodel.gameObject.SetActive(false);
            });
            LeanTween.alpha(panelPlayerStats, 0f, 0.5f).setOnComplete( delegate()
            {
                // This causes problems with the player panels on the right of the screen, since they're mirrored,
                // setting the player stats panel to inactive breaks the layout
                // There's probably no problem with not setting it to inactive at all (probably)
                // Leaving this here just in case

                //panelPlayerStats.gameObject.SetActive(false);
            });


            // this is a nightmare
            // Text fadeout
            var fadeoutTextColor = textBombPower.color;
            fadeoutTextColor.a = 0f;
            LeanTween.value(textBombPower.gameObject, updateTextColorValueCallback, textBombPower.color, fadeoutTextColor, 0.2f);

            // Circle color fade to gray
            var fadeoutImageColor = new Color(0.3f, 0.3f, 0.3f);
            LeanTween.value(textBombPower.gameObject, updateImageColorValueCallback, imagePlayerProfile.color, fadeoutImageColor, 0.2f);
        }


        // weird functions, don't touch
        void updateTextColorValueCallback(Color val)
        {
            textBombPower.color = val;
            textKills.color = val;
        }
        // yes, this one too
        void updateImageColorValueCallback(Color val)
        {
            imagePlayerProfile.color = val;
        }


    }
}
