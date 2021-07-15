using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // Missile data
    private float speed;
    private Vector3 target;
    private int damage;
    private float step;

    private SpriteRenderer spriteRenderer;

    public void Init(float speed, Vector3 target, int damage)
    {
        this.speed = speed;
        this.target = target;
        this.damage = damage;
    }


    void Start()
    {
        // Direction calculations
        Vector3 direction = this.target - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.forward*angle;

        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();


    }

    void Update()
    {
        step =  speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, this.target, step);
        
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        Structure target = collision.transform.parent.GetComponent<Structure>();
        if (target == null)
            return;

        target.Hit(this.damage);

        Transform collider = collision.gameObject.transform;
        Destroy(this.gameObject);
        Instantiate((GameObject)Resources.Load("Prefabs/Explosion", typeof(GameObject)), transform.position, Quaternion.identity);
        //Shake screen, change sprite, etc
    }
}
