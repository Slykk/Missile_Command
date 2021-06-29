using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    

    public Transform rotateTarget;
    Vector3 targetVec = new Vector3();

    // Missile data
    Vector2 target;
    float speed = 3;
    int damage;

    float step;

    public void Init(float speed, Vector2 target, int damage)
    {
        this.speed = speed;
        this.target = target;
        this.damage = damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 targetVec = rotateTarget.position + new Vector3(0, rotateTarget.localScale.y, 0);
        float angle = Mathf.Atan2(rotateTarget.position.y + rotateTarget.localScale.y, rotateTarget.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // Update is called once per frame
    void Update()
    {
        step =  speed * Time.deltaTime;
        
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(0,0), step);
        //Quaternion.RotateTowards(transform.position, targetVec);
        //transform.LookAt(targetVec);
        //RotateTowardsTarget();
    }

    private void RotateTowardsTarget()
    {
        float rotationSpeed = 10f; 
        float offset = 90f;    
        Vector2 direction = rotateTarget.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
