using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // Missile data
    float speed;
    Vector3 target;
    int damage;
    float step;

    public void Init(float speed, Vector3 target, int damage)
    {
        this.speed = speed;
        this.target = target;
        this.damage = damage;
        //transform.SetParent(GameObject.Find("Missiles").transform);
    }


    // Start is called before the first frame update
    void Start()
    {
        // Direction calculations
        Vector3 direction = this.target - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.forward*angle;

        // smoke = Resources.Load("Images/Smoke", typeof(GameObject));


    }

    // Update is called once per frame
    void Update()
    {
        step =  speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, this.target, step);

        
        
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        Transform collider = collision.gameObject.transform;
        if (collision.gameObject.name == "Sprite")
        {
            Instantiate((GameObject)Resources.Load("Prefabs/Smoke", typeof(GameObject)), collider.position, Quaternion.identity);
            Destroy(collision.gameObject); // destroy only for testing
            //Shake screen, change sprite, etc
        }
    }
}
