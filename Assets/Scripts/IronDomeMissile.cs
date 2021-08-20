using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IronDomeMissile : MonoBehaviour
{
    Sequence mySequence = DOTween.Sequence();

    private GameObject gameLogic;

    private Vector3 hitPos;
    private Vector3 lastPos;
    private float timerScale;

    // public float speed = 5f;
    // public float rotateSpeed = 200f;

    // private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        // rb = GetComponent<Rigidbody2D>();
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

        timerScale = Vector3.Distance(hitPos, transform.position);
        Debug.Log(timerScale);

        SetDirection(hitPos);

        Vector3[] missileWayPoints = new[] {Vector3.zero , new Vector3 (-2, 2, 0), hitPos};

        mySequence.Insert(0f, transform.DOPath(missileWayPoints, timerScale / 2f, PathType.CatmullRom, PathMode.Sidescroller2D).SetEase(Ease.OutSine));
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning("I HAVE COLLIDED!!!!!!");
        Transform collider = collision.gameObject.transform;
        Instantiate((GameObject)Resources.Load("Prefabs/Explosion", typeof(GameObject)), transform.position, Quaternion.identity);
        gameLogic.GetComponent<GameLogic>().missileList.Remove(collider.gameObject);
        Destroy(collider.gameObject);
        Destroy(this.gameObject);
        // var number = Instantiate((GameObject)Resources.Load("Prefabs/Number"), transform.position, Quaternion.identity);
        // number.GetComponent<ScorePopup>().Init("8", Vector3.right*2);
    }
}
