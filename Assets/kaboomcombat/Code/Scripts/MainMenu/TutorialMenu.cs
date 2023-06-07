// TutorialMenu class
// ====================================================================================================================
// Handles the tutorial submenu of the main menu


using UnityEngine;
using UnityEngine.InputSystem;


namespace kaboomcombat
{
    public class TutorialMenu : MonoBehaviour
    {
        // Reference to the main menuController
        private MenuController menuController;

        // Reference to the root of this submenu
        public RectTransform panelTutorial;

        private MenuActionAsset inputAsset;


        void Awake () 
        { 
            // Assign references
            menuController = GetComponent<MenuController>();

            // Create a new input asset
            inputAsset = new MenuActionAsset();
        }


        private void OnEnable()
        {
            // Assign action
            inputAsset.Menu.Cancel.performed += SwitchToMainMenu;
            inputAsset.Enable();
        }


        private void OnDisable()
        {
            inputAsset.Menu.Cancel.performed += SwitchToMainMenu;
            inputAsset.Disable();
        }


        // Function to switch to the mainmenu submenu, called when the B or ESC buttons are pressed
        private void SwitchToMainMenu(InputAction.CallbackContext obj)
        {
            menuController.SwitchMenu(MenuState.MAIN);
        }
    }
}
