using System;
using UnityEngine;
using System.Collections;

namespace KingDOM.Event.Debug
{
    [Serializable]
    public class EventsCheckSetup
    {
        public bool Active = true;
        public string EventName = "";
        public EventsCheckObject Caller = new EventsCheckObject();
        public EventsCheckStage Stage = EventsCheckStage.Before;
        public EventsCheckObject Receiver = new EventsCheckObject();
        public ParmValuePairCheck[] ParametersCheck = new ParmValuePairCheck[0];

        [NonSerialized]
        public bool checkOut = false;


        public bool Check(EventsCheckStage stage, SimpleEvent evnt, UnityEngine.Object receiver)
        {
            checkOut = true;
            if (!string.IsNullOrEmpty(EventName))
                checkOut = checkOut && (EventName == evnt.eventName);
            checkOut = checkOut && stage == Stage;
            checkOut = checkOut && Caller.Check(evnt.target as UnityEngine.Object);
            checkOut = checkOut && Receiver.Check(receiver);

            foreach (ParmValuePairCheck parm in ParametersCheck)
            {
                checkOut = checkOut && parm.Check(evnt);
            }
            return checkOut;
        }
    }


}
