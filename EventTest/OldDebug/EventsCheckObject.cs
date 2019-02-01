using System;
using UnityEngine;
using System.Collections;

namespace KingDOM.Event.Debug
{
    [Serializable]
    public class EventsCheckObject
    {

        public Type TargeType = null;
        public UnityEngine.Object Target = null;
        public string TargetName = "";
        public string TargetTag = "";
        public int TargetLayerMask = Physics.AllLayers;
        public int TargetLayerMask2D = Physics2D.AllLayers;

        public bool Check(UnityEngine.Object target)
        {
            if (target == null)
                return false;

            bool ret = true;
            ret = ret && (TargeType == null || target.GetType().IsAssignableFrom(TargeType));
            ret = ret && (Target == null || target == Target);
            ret = ret && (string.IsNullOrEmpty(TargetName) || target.name == TargetName);
            ret = ret && (string.IsNullOrEmpty(TargetTag) || (target is Component && (target as Component).tag == TargetName));
            if (target is Component)
            {
                int layer = ((target as Component).gameObject.layer;
            ret = ret &&
                  (TargetLayerMask == Physics.AllLayers ||
                   (layer & TargetLayerMask) > 0);
                ret = ret &&
                      (TargetLayerMask2D == Physics2D.AllLayers ||
                       (layer & TargetLayerMask2D) > 0);
            }
            
            return ret;
        }
    }
}

