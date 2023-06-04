using UnityEngine;
using UnityEngine.EventSystems;

namespace kaboomcombat
{
    public class NavigateEventTrigger : EventTrigger
    {
        public override void OnSelect(BaseEventData data)
        {
            Invoke("Select", 0.05f);
            ;
        }

        private void Select()
        {
            FindObjectOfType<MainMenuController>().UpdateMenuPointer();
        }
    }
}
