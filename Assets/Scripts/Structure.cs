using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public int hp;
    private Transform missileTarget;

    public bool IsAlive()
    {
        return (hp > 0);
    }

    protected virtual void Awake()
    {
        missileTarget = transform.Find("MissileTarget");
    }

    public Vector3 GetMissileTarget()
    {
        return missileTarget.position;
    }

    public virtual void Hit(int damage)
    {
        hp -= damage;
        if (hp <= 0)
            Kill();
    }

    public virtual void Kill()
    {
        //
    }
}
