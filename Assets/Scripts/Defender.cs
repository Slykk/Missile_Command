using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : Structure
{
    private SpriteRenderer spriteRenderer;
    private string spriteName;
    public int ammo;
    public bool operational;
    public bool reloading;
    private float timer;
    private float reloadTime;

    
    protected override void Awake()
    {
        base.Awake();
        
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        spriteName = spriteRenderer.sprite.name;


        operational = true;
        reloading = false;

        reloadTime = 2f;
        timer = reloadTime;
        ammo = 10;
    }

    void Update()
    {
        if (reloading)
        {
            operational = false;
            timer -= reloadTime * Time.deltaTime;
            if (timer <= 0)
            {
                timer = reloadTime;
                reloading = false;
                operational = true;
            }
        }
        if (ammo <= 0 && operational)
        {
            operational = false;
        }
    }

    public override void Hit(int damage)
    {
        base.Hit(damage);
    }

    public override void Kill()
    {
        base.Kill();
        spriteRenderer.sprite = (Sprite)Resources.Load("Images/" + spriteName + "_Destroyed", typeof(Sprite));
    }
}
