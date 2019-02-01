using System.Collections.Generic;

namespace KingDOM.Event
{
    /// <summary>
    /// Simple Event. Link to this class is transmitted when an event occurs into the methods of processing events.
    /// </summary>
    public class SimpleEvent {
        /// <summary>
        /// Source of event
        /// </summary>
        public readonly object	target;
        /// <summary>
        /// Event name is also a unique key by which the event links with handlers subscribing.
        /// </summary>
        public readonly string eventName;
        /// <summary>
        /// Parameters which are sent with the event
        /// </summary>
        public readonly Dictionary<string, object> args = new Dictionary<string, object>();
        /// <summary>
        /// If flag is set true, this will stop event propagation 
        /// </summary>
        public bool stop = false;
	
        /// <summary>
        /// Constructor to make Event
        /// </summary>
        /// <param name="eventName">Event name (unique key for each event)</param>
        /// <param name="target"> Source of event</param>
        /// <param name="args"> List of parameters which includes any number of pairs (keyName string / value object)</param>
        public SimpleEvent(string eventName, object target, params object[]	args){
            this.eventName = eventName;
            this.target = target;
            for (int i = 0; i < args.Length / 2; i++){
                if (args[i * 2].GetType() == typeof(string) && !this.args.ContainsKey((string) args[i*2])){
                    this.args.Add((string) args[i*2], args[i*2 + 1]);
                }
				
            }
        }
	
        /// <summary>
        /// Get parameter value by the key
        /// </summary>
        /// <param name="key">Parameter name</param>
        /// <returns>Parameter value, if parameter does not exist, it returns null.</returns>
        public object GetParm(string key){
            if (args.ContainsKey(key))
                return args[key];
		
            return null;
        }
        /// <summary>
        /// Get parameter value by the key with checking Type
        /// </summary>
        /// <typeparam name="T">Parameter type</typeparam>
        /// <param name="key">Parameter name</param>
        /// <returns>Parameter value, if parameter does not exist, it returns null.</returns>
        public T GetParm<T>(string key){
            T	val = default(T);
            if (args.ContainsKey(key) && args[key] is T)
                val = (T)args[key];
		
            return val;
        }
        /// <summary>
        /// Check parameter existance
        /// </summary>
        /// </summary>
        /// <param name="key">Parameter name</param>
        /// <returns>True if parameter exists</returns>
        public bool ExistParm(string key)
        {
            return args.ContainsKey(key);
        }
        /// <summary>
        /// Check parameter existance  with checking Type
        /// </summary>
        /// <typeparam name="T">Parameter type</typeparam>
        /// <param name="key">Parameter name</param>
        /// <returns>True if parameter exists.</returns>
        public bool ExistParm<T>(string key)
        {
            return args.ContainsKey(key) && args[key] is T;
        }
        

    }
}