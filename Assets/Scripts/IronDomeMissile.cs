using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IronDomeMissile : MonoBehaviour
{
    Sequence mySequence = DOTween.Sequence();

    private Vector3 hitPos;
    private Vector3 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // update sprite angle according to movement direction
        SetDirection(lastPos);
        lastPos = transform.position;


    }

    public void Init(Vector3 hitPos)
    {
        this.hitPos = hitPos;

        SetDirection(hitPos);

        Vector3[] missileWayPoints = new[] {Vector3.zero , new Vector3 (-2, 2, 0), hitPos};

        mySequence.Insert(0f, transform.DOPath(missileWayPoints, 2f, PathType.CatmullRom).SetEase(Ease.OutSine));
    }

    private void SetDirection(Vector3 relativePosition)
    {
        // calculate missile hit position
        Debug.LogWarning("hit position vector is: " + relativePosition);
        Vector3 direction = transform.position - relativePosition;
        Debug.LogWarning("direction vector is: " + direction);
        direction.Normalize();
        Debug.LogWarning("NORMALIZED direction vector is: " + direction);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Debug.LogWarning("angle is: " + angle);
        transform.eulerAngles = Vector3.forward*angle;
    }
}
