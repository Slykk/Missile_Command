using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderMissile : MonoBehaviour
{
    private float blastRadius;
    private float blastSpeed;
    private float speed;
    private Vector3 target;
    private float step;



    public void Init(float speed, Vector3 target, float blastRadius, float blastSpeed)
    {
        this.speed = speed;
        this.target = target;
        this.blastRadius = blastRadius;
        this.blastSpeed = blastSpeed;
    }

    void Start()
    {
        // Direction calculations
        Vector3 direction = target - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.forward*angle;
    }

    void Update()
    {
        step =  speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target, step);

        if (transform.position == target)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);
            foreach (var collider in colliders)
            {
                if (collider.tag == "Missile")
                {
                    Destroy(collider.gameObject);
                    Instantiate((GameObject)Resources.Load("Prefabs/SmallExplosion", typeof(GameObject)), collider.transform.position, Quaternion.identity);
                }
            }

            Debug.LogWarning("Destination arrived!");
            Instantiate((GameObject)Resources.Load("Prefabs/Explosion", typeof(GameObject)), transform.position, Quaternion.identity);
            //Instantiate((GameObject)Resources.Load("Prefabs/BuildingExplosionSplatter", typeof(GameObject)), transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Transform collider = collision.gameObject.transform;
        Instantiate((GameObject)Resources.Load("Prefabs/Explosion", typeof(GameObject)), transform.position, Quaternion.identity);
        Destroy(collider.gameObject);
        Destroy(this.gameObject);
        // var number = Instantiate((GameObject)Resources.Load("Prefabs/Number"), transform.position, Quaternion.identity);
        // number.GetComponent<ScorePopup>().Init("8", Vector3.right*2);
    }
}
