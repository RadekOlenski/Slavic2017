using System.Collections.Generic;
using System.Linq;

namespace Events
{
    //Abstract class for defining common part of all events. We can later split it on small parts, depending of what we will need
    public abstract class GameEvent
    {
        protected List<object> args;

        private string BuildStringFromList()
        {
            return args == null ? "" : args.Aggregate<object, string>(null, (current, o) => current + (o + ", "));
        }

        public virtual void DebugEvent()
        {
            //UnityEngine.Debug.Log("Event: " + this + " invoked, with parameters: " + BuildStringFromList());
        }
    }
}