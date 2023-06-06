using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace kaboomcombat
{
    public class TutorialMenu : MonoBehaviour
    {
        private MenuController menuController;

        public RectTransform panelTutorial;

        private MenuActionAsset inputAsset;


        void Awake () 
        { 
            menuController = GetComponent<MenuController>();

            inputAsset = new MenuActionAsset();
        }


        private void OnEnable()
        {
            inputAsset.Menu.Cancel.performed += SwitchToMainMenu;
            inputAsset.Enable();
        }


        private void OnDisable()
        {
            inputAsset.Menu.Cancel.performed += SwitchToMainMenu;
            inputAsset.Disable();
        }


        private void SwitchToMainMenu(InputAction.CallbackContext obj)
        {
            menuController.SwitchMenu(MenuState.MAIN);
        }
    }
}
