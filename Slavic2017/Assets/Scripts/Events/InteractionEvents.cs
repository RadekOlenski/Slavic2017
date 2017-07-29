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

        public class EnableMovementModeEvent : GameEvent
        {
            public bool MovementModeEnabled { get; set; }

            public EnableMovementModeEvent(bool movementModeEnabled)
            {
                MovementModeEnabled = movementModeEnabled;
                args = new List<object> { MovementModeEnabled };
            }
        }
    }
}