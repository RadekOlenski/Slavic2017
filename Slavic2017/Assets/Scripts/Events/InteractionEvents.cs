using System.Collections.Generic;

namespace Events
{
    public class InteractionEvents : GameEvent
    {
        public class EnableFlashlightModeEvent : GameEvent
        {
            public bool FlashlightModeEnabled { get; set; }

            public EnableFlashlightModeEvent(bool flashlightModeEnabled)
            {
                FlashlightModeEnabled = flashlightModeEnabled;
                args = new List<object> {FlashlightModeEnabled};
            }
        }
    }
}