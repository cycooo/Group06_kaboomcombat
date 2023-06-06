using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


namespace kaboomcombat
{
    public class MenuController : MonoBehaviour
    {
        private MainMenu mainMenu;
        private TutorialMenu tutorialMenu;
        private OptionsMenu optionsMenu;

        private bool animationFinished = true;
        private float menuSwitchTime = 0.5f;

        public static MenuState menuState = MenuState.MAIN;
        
        private float spawnInterval = 1f;
        private Vector2 xBounds = new Vector2(-10f, 10f);
        private Vector2 zBounds = new Vector2(-12f, 16f);

        public GameObject[] spawnLimits = new GameObject[2];

        // List of prefabs to instantiate for the background
        public List<GameObject> bombList = new List<GameObject>();
        public Camera mainCamera;

        public EventSystem eventSystem;


        void Awake()
        {
            mainMenu = GetComponent<MainMenu>();
            tutorialMenu = GetComponent<TutorialMenu>();
            optionsMenu = GetComponent<OptionsMenu>();

            DataManager.gameState = GameState.MENU;

            eventSystem = FindObjectOfType<EventSystem>();

            Physics.gravity = new Vector3(0f, -1f, 0f);

            AssignSpawnLimits();
            SpawnBackgroundObject();
        }


        public void SwitchMenu(MenuState targetMenu)
        {
            if(animationFinished)
            {
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


        private IEnumerator SetActiveWithDelay(GameObject gameObject, bool active, float delay)
        {
            yield return new WaitForSeconds(delay);

            gameObject.SetActive(active);
        }


        public void StartGame()
        {
            if(DataManager.playerListStatic.Count >= 2)
            {
                SceneManager.LoadScene("MainGame");
            }
        }

        public void StartGameForced()
        {
            SceneManager.LoadScene("MainGame");
        }


        private void AssignSpawnLimits()
        {
            xBounds = new Vector2(spawnLimits[0].transform.position.x, spawnLimits[1].transform.position.x);
            zBounds = new Vector2(spawnLimits[0].transform.position.z, spawnLimits[1].transform.position.z);
        }


        private void SpawnBackgroundObject()
        {
            float xPos = Random.Range(xBounds.x, xBounds.y);
            float zPos = Random.Range(zBounds.x, zBounds.y);

            float torqueValue = 30f;
            Vector3 torque = new Vector3(Random.Range(-torqueValue, torqueValue), Random.Range(-torqueValue, torqueValue), Random.Range(-torqueValue, torqueValue));

            GameObject bombInstance = Instantiate(bombList[Random.Range(0, 3)], new Vector3(xPos, 12f, zPos), Random.rotation);
            bombInstance.GetComponent<Rigidbody>().drag = Random.Range(0f, 2f);
            bombInstance.GetComponent<Rigidbody>().AddTorque(torque);

            Invoke("SpawnBackgroundObject", spawnInterval);
        }
    }
}
