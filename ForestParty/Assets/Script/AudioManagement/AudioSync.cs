using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class AudioSync : NetworkBehaviour
{
    private AudioSource sound;

    private void Start()
    {
        // Chỉ khởi tạo sound trên máy khách
        if (IsClient)
        {
            sound = GetComponent<AudioSource>();
        }
    }
    private void Update()
    {
        if(Input.GetButtonDown("Jump")){
            if (IsLocalPlayer)
            {
            RequestPlaySoundServerRpc();
            }
        }
    }
    [ServerRpc]
    private void RequestPlaySoundServerRpc()
    {
        PlaySoundClientRpc();
    }
    [ClientRpc]
    private void PlaySoundClientRpc()
    {
        sound.Play();
    }    
}
