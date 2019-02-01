using System;
using UnityEngine;
using System.Collections;

namespace KingDOM.Event.Debug
{
    [Flags]
    public enum EventsCheckStage
    {
        None = 0,
        Before = 1,
        After = 2,
        Error = 4,
        Stop = 8
    }
}
