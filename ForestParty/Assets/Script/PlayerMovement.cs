using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private AudioSource sound;
    public float speed;
    private Rigidbody2D player;
    private SpriteRenderer sprite;
    private Animator anim;
    private float dirX = 0f;
    private float dirY = 0f;
    private void Start()
    {
        player = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    
    private void Update()
    {
        if(!IsOwner)
        {
            return;
        }
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");
        player.velocity = new Vector2(dirX * speed, dirY * speed);
        ChangeDir();
        if(Input.GetButtonDown("Jump"))
        {
            Active();
            sound.Play();
            Invoke("UnActive", 1f);
        }
    }
    private void Active()
    {
        anim.SetBool("Active", true);
    }
    private void UnActive()
    {
        anim.SetBool("Active", false);
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
