using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Structure
{
    private SpriteRenderer spriteRenderer;
    private string spriteName;

    protected override void Awake()
    {
        base.Awake();
        
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        spriteName = spriteRenderer.sprite.name;
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
