using System;
using UnityEngine;
using System.Collections;
using KingDOM.Event;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SphereCollider))]
public class Planet2 : Planet {

    void CreateFirework(float power)
    {
        if (FireworkObject == null) return;
        GameObject go = Instantiate(FireworkObject) as GameObject;
        Fireworks fireworks = go.GetComponent<Fireworks>();
        if (fireworks == null)
        {
            DestroyImmediate(go);
            return;
        }
        fireworks.SetPower(power);
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
    }
    void CreateShield(float power)
    {
        if (ShieldObject == null) return;
        GameObject go = Instantiate(ShieldObject) as GameObject;
        Bubble shield = go.GetComponent<Bubble>();
        if (shield == null)
        {
            DestroyImmediate(go);
            return;
        }
        shield.SetPower(power);
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
    }
    void CreateExplosion(float power)
    {
        if (ExplosiveObject == null) return;
        GameObject go = Instantiate(ExplosiveObject) as GameObject;
        Boom boom = go.GetComponent<Boom>();
        if (boom == null)
        {
            DestroyImmediate(go);
            return;
        }
        boom.SetPower(power);
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
    }

    override public void hnFireworks(SimpleEvent evnt)
    {
        float power = 1;
        if (evnt != null && evnt.ExistParm<float>(EventParm.V_POWER))
            power = evnt.GetParm<float>(EventParm.V_POWER);
        CreateFirework(power);
    }
    override public void hnShield(SimpleEvent evnt)
    {
        float power = 1;
        if (evnt != null && evnt.ExistParm<float>(EventParm.V_POWER))
            power = evnt.GetParm<float>(EventParm.V_POWER);
        CreateShield(power);
    }
    override public void hnExplosion(SimpleEvent evnt)
    {
        float power = 1;
        if (evnt != null && evnt.ExistParm<float>(EventParm.V_POWER))
            power = evnt.GetParm<float>(EventParm.V_POWER);
        CreateExplosion(power);
    }



}
