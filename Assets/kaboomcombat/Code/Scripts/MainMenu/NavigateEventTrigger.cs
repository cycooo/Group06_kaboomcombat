// This class handles the event of the player moving their selection on the main menu
// (ex. from play to tutorial or options)

using UnityEngine.EventSystems;

namespace kaboomcombat
{
    public class NavigateEventTrigger : EventTrigger
    {
        public override void OnSelect(BaseEventData data)
        {
            // Add a delay because other classes are too slow and it errors out
            Invoke("Select", 0.05f);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            Invoke("Submit", 0f);
        }

        public override void OnCancel(BaseEventData eventData)
        {
            Invoke("Cancel", 0f);
        }

        private void Select()
        {
            // Call the function to update the menu pointer
            FindObjectOfType<MainMenu>().UpdateMenuPointer();
            SoundSystem.instance.PlaySound(Sounds.UI_MOVE);
        }

        private void Submit()
        {
            SoundSystem.instance.PlaySound(Sounds.UI_SELECT);
        }

        private void Cancel()
        {
            SoundSystem.instance.PlaySound(Sounds.UI_CANCEL);
        }
    }
}
