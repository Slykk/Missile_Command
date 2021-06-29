using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private Transform missileTarget;

    void Awake()
    {
        missileTarget = transform.Find("MissileTarget");
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector3 GetMissileTarget()
    {
        return missileTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
