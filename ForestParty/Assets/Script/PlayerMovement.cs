using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private AudioSource sound;
    private NetworkVariable<bool> isFlipped = new NetworkVariable<bool>(); // Biến đồng bộ để flip
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
        isFlipped.OnValueChanged += OnFlipChanged;
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

        // Xác định xem sprite có nên flip dựa trên dirX
        bool shouldFlip = dirX > 0;
        if (dirX != 0 && shouldFlip != sprite.flipX)
        {
            // Gửi RPC lên máy chủ để yêu cầu cập nhật
            RequestFlipServerRpc(shouldFlip);
        }
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

    private void OnFlipChanged(bool oldValue, bool newValue)
    {
        sprite.flipX = newValue;
    }

    // RPC này được gọi từ client nhưng thực thi trên máy chủ
    [ServerRpc]
    private void RequestFlipServerRpc(bool newFlipState)
    {
        // Cập nhật NetworkVariable trên máy chủ
        isFlipped.Value = newFlipState;
    }
}
