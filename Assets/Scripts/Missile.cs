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

    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteArray;

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

        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();


    }

    // Update is called once per frame
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
        Instantiate((GameObject)Resources.Load("Prefabs/BuildingExplosion", typeof(GameObject)), collider.position - new Vector3(0, 0.5f, 0), Quaternion.identity);
        Instantiate((GameObject)Resources.Load("Prefabs/BuildingExplosionSplatter", typeof(GameObject)), collider.position, Quaternion.identity);
        //Shake screen, change sprite, etc
    }
}
