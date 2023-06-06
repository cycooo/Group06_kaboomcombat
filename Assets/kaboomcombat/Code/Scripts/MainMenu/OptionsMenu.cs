using kaboomcombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace kaboomcombat
{
    public class OptionsMenu : MonoBehaviour
    {
        private MenuController menuController;

        public RectTransform panelOptions;

        private MenuActionAsset inputAsset;


        void Awake()
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
