// MenuController class
// ====================================================================================================================
// Main class of the main menu; handles interactions and animations between submenus, among others


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


namespace kaboomcombat
{
    public class MenuController : MonoBehaviour
    {
        // References to all the submenus
        private MainMenu mainMenu;
        private TutorialMenu tutorialMenu;
        private OptionsMenu optionsMenu;

        // Parameters used for the transition between two submenus
        private bool animationFinished = true;
        private float menuSwitchTime = 0.5f;

        // Enum used to keep track of the current submenu
        public static MenuState menuState = MenuState.MAIN;
        
        // Parameters used for the spawning of bombs in the background
        private float spawnInterval = 1f;
        // Defining the spawn location of the bombs
        private Vector2 xBounds = new Vector2(-10f, 10f);
        private Vector2 zBounds = new Vector2(-12f, 16f);

        // Array that stores the coordinates of the spawn area for the bombs in the background
        public GameObject[] spawnLimits = new GameObject[2];

        // List of prefabs to instantiate for the background
        public List<GameObject> bombList = new List<GameObject>();
        public Camera mainCamera;

        // Eventsystem reference
        public EventSystem eventSystem;


        void Awake()
        {
            // Get references
            mainMenu = GetComponent<MainMenu>();
            tutorialMenu = GetComponent<TutorialMenu>();
            optionsMenu = GetComponent<OptionsMenu>();

            eventSystem = FindObjectOfType<EventSystem>();

            // Assign the gamestate to MENU, since we're in the main menu
            DataManager.gameState = GameState.MENU;
            
            // Set the gravity for the falling bombs in the background
            Physics.gravity = new Vector3(0f, -1f, 0f);


            AssignSpawnLimits();
            SpawnBackgroundObject();
        }


        // Function that loads the MainGame scene if there are enough players
        public void StartGame()
        {
            if (DataManager.playerListStatic.Count >= 2)
            {
                SceneManager.LoadScene("MainGame");
            }
        }


        // Function that forcefully loads the MainGame scene (for debugging)
        public void StartGameForced()
        {
            SceneManager.LoadScene("MainGame");
        }


        // Assign the coordinates for the spawn area of the falling bombs
        private void AssignSpawnLimits()
        {
            xBounds = new Vector2(spawnLimits[0].transform.position.x, spawnLimits[1].transform.position.x);
            zBounds = new Vector2(spawnLimits[0].transform.position.z, spawnLimits[1].transform.position.z);
        }


        // Function that switches between submenus with a sliding animation
        public void SwitchMenu(MenuState targetMenu)
        {
            // Don't do anything unless the previous animation has finished
            if(animationFinished)
            {
                // No time to comment all this, should be fine for the moment since it's not that important
                // This is pretty messy but it works, it just switches between MenuStates according to what menu
                // you want to transition to and animates a slide
                switch(menuState)
                {
                    case MenuState.MAIN:
                        switch(targetMenu)
                        {
                            case MenuState.TUTORIAL:
                                animationFinished = false;

                                LTDescr mainTween = LeanTween.move(mainMenu.panelMain, new Vector3(-630f, -360f, 0f), menuSwitchTime);
                                mainTween.setEaseOutQuart();
                                mainTween.setOnComplete(delegate ()
                                {
                                    mainMenu.panelMain.gameObject.SetActive(false);
                                    mainMenu.enabled = false;
                                    animationFinished = true;
                                });

                                break;
                            case MenuState.OPTIONS:
                                animationFinished = false;

                                mainTween = LeanTween.move(mainMenu.panelMain, new Vector3(640f, 380f, 0f), menuSwitchTime);
                                mainTween.setEaseOutQuart();
                                mainTween.setOnComplete(delegate ()
                                {
                                    mainMenu.panelMain.gameObject.SetActive(false);
                                    mainMenu.enabled = false;
                                    animationFinished = true;
                                });

                                break;
                        }

                        break;
                    case MenuState.TUTORIAL:
                        animationFinished = false;

                        LTDescr tutorialTween = LeanTween.move(tutorialMenu.panelTutorial, new Vector3(1920f, -360f, 0f), menuSwitchTime);
                        tutorialTween.setEaseOutQuart();
                        tutorialTween.setOnComplete(delegate ()
                        {
                            tutorialMenu.panelTutorial.gameObject.SetActive(false);
                            tutorialMenu.enabled = false;
                            animationFinished = true;
                        });

                        break;
                    case MenuState.OPTIONS:
                        animationFinished = false;

                        LTDescr optionsTween = LeanTween.move(optionsMenu.panelOptions, new Vector3(640f, -1100f, 0f), menuSwitchTime);
                        optionsTween.setEaseOutQuart();
                        optionsTween.setOnComplete(delegate ()
                        {
                            optionsMenu.panelOptions.gameObject.SetActive(false);
                            optionsMenu.enabled = false;
                            animationFinished = true;
                        });

                        break;
                }

                switch(targetMenu)
                {
                    case MenuState.MAIN:
                        animationFinished = false;

                        LTDescr mainTween = LeanTween.move(mainMenu.panelMain, new Vector3(640f, -360f, 0f), menuSwitchTime);
                        mainTween.setEaseOutQuart();
                        mainTween.setOnComplete(delegate ()
                        {
                            menuState = MenuState.MAIN;
                            animationFinished = true;
                        });

                        mainMenu.panelMain.gameObject.SetActive(true);
                        mainMenu.enabled = true;

                        break;
                    case MenuState.TUTORIAL:
                        animationFinished = false;

                        LTDescr tutorialTween = LeanTween.move(tutorialMenu.panelTutorial, new Vector3(640f, -360f, 0f), menuSwitchTime);
                        tutorialTween.setEaseOutQuart();
                        tutorialTween.setOnComplete(delegate ()
                        {
                            menuState = MenuState.TUTORIAL;
                            animationFinished = true;
                        });

                        tutorialMenu.panelTutorial.gameObject.SetActive(true);
                        tutorialMenu.enabled = true;

                        break;
                    case MenuState.OPTIONS:
                        animationFinished = false;

                        LTDescr optionsTween = LeanTween.move(optionsMenu.panelOptions, new Vector3(640f, -360f, 0f), menuSwitchTime);
                        optionsTween.setEaseOutQuart();
                        optionsTween.setOnComplete(delegate ()
                        {
                            menuState = MenuState.OPTIONS;
                            animationFinished = true;
                        });

                        optionsMenu.panelOptions.gameObject.SetActive(true);
                        optionsMenu.enabled = true;

                        break;
                }
            }
        }


        // Function that gets called every second to spawn a random Bomb prefab at a random location 
        // within the area defined at the start of the class
        private void SpawnBackgroundObject()
        {
            // Get a random position in the range of the points defined earlier
            float xPos = Random.Range(xBounds.x, xBounds.y);
            float zPos = Random.Range(zBounds.x, zBounds.y);

            // Add a random torque (rotational force) to the bombs
            float torqueValue = 30f;
            Vector3 torque = new Vector3(Random.Range(-torqueValue, torqueValue), Random.Range(-torqueValue, torqueValue), Random.Range(-torqueValue, torqueValue));

            // Instantiate the bomb and add all the parameters to it
            GameObject bombInstance = Instantiate(bombList[Random.Range(0, 3)], new Vector3(xPos, 12f, zPos), Random.rotation);
            bombInstance.GetComponent<Rigidbody>().drag = Random.Range(0f, 2f);
            bombInstance.GetComponent<Rigidbody>().AddTorque(torque);

            // Call the function again after one second
            Invoke("SpawnBackgroundObject", spawnInterval);
        }
    }
}
