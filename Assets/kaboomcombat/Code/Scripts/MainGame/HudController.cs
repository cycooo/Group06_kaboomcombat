// HudController Class
// ====================================================================================================================
// Handles the in-game hud (timer, player information panels etc.)


using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace kaboomcombat
{
    public class HudController : MonoBehaviour
    {
        private bool open = false;
        private float hudOpenTime = 0.5f;

        // References
        private SessionManager sessionManager;
        public TextMeshProUGUI textTimer;

        public GameObject panelHud;
        public GameObject panelCountdown;
        public RectTransform panelNumber;
        public RectTransform panelGo;
        public Image imageNumber;

        public Sprite[] numberSprites = new Sprite[3];

        public RectTransform panelLeft;
        public RectTransform panelRight;
        public RectTransform panelTimer;

        // Reference to the array used to store all player information panels for easy access
        public GameObject[] panelPlayerHudArray = new GameObject[4];


        void Awake()
        {
            // Assign the sessionManager
            sessionManager = FindObjectOfType<SessionManager>();

            // First, disable all player information panels to avoid errors on level load
            DisablePlayerPanels();
            panelHud.SetActive(false);
            // Initialize the hud after a small delay to give the player info panels time to get the information they need
            Invoke("InitializeHud", 0.2f);

            // If the hud is set to be closed on start, set the panel's positions appropriately
            if(!open)
            {
                panelLeft.anchoredPosition = new Vector3(-250f, -360f, 0f);
                panelRight.anchoredPosition = new Vector3(250f, -360f, 0f);
                panelTimer.anchoredPosition = new Vector3(0f, 60f, 0f);
            }
        }

        
        // Function used to update the timer each frame
        public void UpdateTimer(float time)
        {
            // Calculate minutes and seconds from the total time float
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);

            // Modify the timer's text with the current time remaining
            textTimer.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }


        // Function that initializes the hud.
        // Determine the amount of player information panels to display, what to display on them.
        // This function is called when the level first starts, otherwise we get errors from the information panels
        // because variables weren't assigned
        public void InitializeHud()
        {
            // Find out how many player information panels we need, set the id for each one and enable only the ones
            // that are needed (Ex. With 2 players, only 2 information panels are enabled)
            // We use DataManager's playerListStatic count to find out the total number of players
            for(int i = 0; i < DataManager.playerListStatic.Count; i++)
            {
                panelPlayerHudArray[i].GetComponent<PanelPlayerHud>().playerId = i;
                panelPlayerHudArray[i].SetActive(true);
            }
        }


        // Function to disable every player panel
        public void DisablePlayerPanels()
        {
            foreach (GameObject playerPanelHud in panelPlayerHudArray)
            {
                playerPanelHud.SetActive(false);
            }
        }


        // Function that animates the hud opening
        public void OpenHud()
        {
            panelHud.SetActive(true);

            LTDescr leftTween = LeanTween.move(panelLeft, new Vector3(0f, -360f, 0f), hudOpenTime);
            leftTween.setEaseOutQuart();

            LTDescr rightTween = LeanTween.move(panelRight, new Vector3(0f, -360f, 0f), hudOpenTime);
            rightTween.setEaseOutQuart();

            LTDescr timerTween = LeanTween.move(panelTimer, new Vector3(0f, -20f, 0f), hudOpenTime);
            timerTween.setEaseOutQuart();

            open = true;
        }


        // Function that animates the hud closing
        public void CloseHud()
        {
            LTDescr leftTween = LeanTween.move(panelLeft, new Vector3(-250f, -360f, 0f), hudOpenTime);
            leftTween.setEaseOutQuart();

            LTDescr rightTween = LeanTween.move(panelRight, new Vector3(250f, -360f, 0f), hudOpenTime);
            rightTween.setEaseOutQuart();

            LTDescr timerTween = LeanTween.move(panelTimer, new Vector3(0f, 60f, 0f), hudOpenTime);
            timerTween.setEaseOutQuart();
            timerTween.setOnComplete(delegate ()
            {
                panelHud.SetActive(false);
                open = false;
            });
        }


        // Function that animates the start countdown
        public IEnumerator StartHudCountdown()
        {
            // Wait for 1 second
            yield return new WaitForSeconds(1);

            // Set the countdown panel to active
            panelCountdown.SetActive(true);

            // Define leantween descriptions
            LTDescr numberRotationTween;
            LTDescr numberScaleTween;
            LTDescr numberFadeTween;


            // 3
            // Set initial parameters before animation
            LeanTween.alpha(panelNumber, 0f, 0f);
            LeanTween.scale(panelNumber, new Vector3(2f, 2f, 2f), 0f);
            panelNumber.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
            // Apply the correct sprite
            imageNumber.sprite = numberSprites[2];

            // Animate the panel rotating, scaling and fading in, before fading out after
            LeanTween.alpha(panelNumber, 1f, 0.2f);
            numberScaleTween = LeanTween.scale(panelNumber, new Vector3(1f, 1f, 1f), 0.3f).setEaseOutQuart();
            numberRotationTween = LeanTween.rotateAround(panelNumber, Vector3.forward, 180f, 0.3f);
            numberRotationTween.setEaseOutQuart();
            numberRotationTween.setOnComplete(delegate ()
            {
                numberFadeTween = LeanTween.alpha(panelNumber, 0f, 0.7f);
            });
            

            // Wait for one second
            yield return new WaitForSeconds(1);


            // 2
            // Set initial parameters before animation
            LeanTween.alpha(panelNumber, 0f, 0f);
            LeanTween.scale(panelNumber, new Vector3(2f, 2f, 2f), 0f);
            panelNumber.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
            // Apply the correct sprite
            imageNumber.sprite = numberSprites[1];

            // Animate the panel rotating, scaling and fading in, before fading out after
            LeanTween.alpha(panelNumber, 1f, 0.2f);
            numberScaleTween = LeanTween.scale(panelNumber, new Vector3(1f, 1f, 1f), 0.3f).setEaseOutQuart();
            numberRotationTween = LeanTween.rotateAround(panelNumber, Vector3.forward, 180f, 0.3f);
            numberRotationTween.setEaseOutQuart();
            numberRotationTween.setOnComplete(delegate ()
            {
                numberFadeTween = LeanTween.alpha(panelNumber, 0f, 0.7f);
            });


            // Wait for one second
            yield return new WaitForSeconds(1);


            // 1
            // Set initial parameters before animation
            LeanTween.alpha(panelNumber, 0f, 0f);
            LeanTween.scale(panelNumber, new Vector3(2f, 2f, 2f), 0f);
            panelNumber.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
            // Apply the correct sprite
            imageNumber.sprite = numberSprites[0];

            // Animate the panel rotating, scaling and fading in, before fading out after
            LeanTween.alpha(panelNumber, 1f, 0.2f);
            numberScaleTween = LeanTween.scale(panelNumber, new Vector3(1f, 1f, 1f), 0.3f).setEaseOutQuart();
            numberRotationTween = LeanTween.rotateAround(panelNumber, Vector3.forward, 180f, 0.3f);
            numberRotationTween.setEaseOutQuart();
            numberRotationTween.setOnComplete(delegate ()
            {
                numberFadeTween = LeanTween.alpha(panelNumber, 0f, 0.7f);
            });


            // Wait for one second
            yield return new WaitForSeconds(1);
            // Start the game as the countdown is over
            sessionManager.StartGame();


            // Set the number panel inactive and the "go" panel active
            panelNumber.gameObject.SetActive(false);
            panelGo.gameObject.SetActive(true);


            // Go
            // Set initial parameters before animation
            LeanTween.alpha(panelGo, 0f, 0f);
            LeanTween.scale(panelGo, new Vector3(2f, 2f, 2f), 0f);
            panelGo.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));

            // Animate the panel rotating, scaling and fading in, before fading out after 1 second
            LeanTween.alpha(panelGo, 1f, 0.2f);
            numberScaleTween = LeanTween.scale(panelGo, new Vector3(1f, 1f, 1f), 0.3f).setEaseOutQuart();
            numberRotationTween = LeanTween.rotateAround(panelGo, Vector3.forward, 180f, 0.3f);
            numberRotationTween.setEaseOutQuart();

            yield return new WaitForSeconds(1);
               
            numberFadeTween = LeanTween.alpha(panelGo, 0f, 0.7f);

            yield return new WaitForSeconds(1);

            // Disbale the two remaining panels in the countdown hud
            panelGo.gameObject.SetActive(false);
            panelCountdown.SetActive(false);
        }
    }
}
