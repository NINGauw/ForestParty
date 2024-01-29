using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public float speed;
    private Rigidbody2D player;
    private SpriteRenderer sprite;
    private float dirX = 0f;
    private float dirY = 0f;
    private void Start()
    {
        player = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");
        player.velocity = new Vector2(dirX * speed, dirY * speed);
        ChangeDir();
    }
    
    private void ChangeDir()
    {
        if(dirX > 0)
        {
            sprite.flipX = true;
        }
        if(dirX < 0)
        {
            sprite.flipX = false;
        }
    }

}
