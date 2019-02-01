using System;

namespace KingDOM.Event.Debug
{
    public class EventCheckArgs:EventArgs
    {
        public EventsCheckStage Stage;
        public SimpleEvent Event;
        public object Receiver;
        public int Priority;

    }
}