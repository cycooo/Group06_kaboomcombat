// This class handles the event of the player moving their selection on the main menu
// (ex. from play to tutorial or options)

using UnityEngine.EventSystems;

namespace kaboomcombat
{
    public class NavigateEventTrigger : EventTrigger
    {
        private SoundSystem soundSystem;

        private void Awake()
        {
            soundSystem = FindObjectOfType<SoundSystem>();
        }

        public override void OnSelect(BaseEventData data)
        {
            // Add a delay because other classes are too slow and it errors out
            Invoke("Select", 0.05f);
        }

        private void Select()
        {
            // Call the function to update the menu pointer
            FindObjectOfType<MainMenu>().UpdateMenuPointer();
            soundSystem.PlaySound(Sounds.UI_MOVE);
        }
    }
}
