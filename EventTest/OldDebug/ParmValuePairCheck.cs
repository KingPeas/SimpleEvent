using System;

namespace KingDOM.Event.Debug
{
    [Serializable]
    public class ParmValuePairCheck
    {
        
        public string ParmName = "";

        public Type ValueType = null;

        public bool CheckValue = false;

        public object Value = null;

        public bool Check(SimpleEvent evnt)
        {
            if (evnt == null)
                return false;
            bool ret = true;

            ret = ret && (string.IsNullOrEmpty(ParmName) || evnt.ExistParm(ParmName));
            if (ret && !string.IsNullOrEmpty(ParmName))
            {
                object val = evnt.GetParm(ParmName);
                ret = ret && (ValueType == null || val.GetType().IsAssignableFrom(val.GetType()));
                ret = ret && (!CheckValue || Value == val);
            }
            return ret;
        }

    }


}