using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BackGroundAudioManagement : NetworkBehaviour
{
    public static BackGroundAudioManagement Instance {get; private set;}
    private AudioSource backgroundMusic1;
    private AudioSource backgroundMusic2;
    private AudioSource backgroundMusic3;

    void Start()
    {
        Instance = this;
        if (IsClient){
            backgroundMusic1 = GetComponent<AudioSource>();
            backgroundMusic2 = GameObject.Find("Music1").GetComponent<AudioSource>();
            backgroundMusic3 = GameObject.Find("Music2").GetComponent<AudioSource>();
        }
        
        RequestPlaySoundServerRpc(1);
    }
    public void ChangeBackground(int audioIndex)
    {
        RequestPlaySoundServerRpc(audioIndex);
    }
    [ServerRpc]
    private void RequestPlaySoundServerRpc(int audioIndex){
        PlaySoundClientRpc(audioIndex);
    }
    [ClientRpc]
    private void PlaySoundClientRpc(int audioIndex){
        switch(audioIndex){
            case 0:
            backgroundMusic2.Stop();
            backgroundMusic3.Stop();
            backgroundMusic1.Play();
            break;
            case 1:
            backgroundMusic1.Stop();
            backgroundMusic3.Stop();
            backgroundMusic2.Play();
            break;
            case 2:
            backgroundMusic1.Stop();
            backgroundMusic2.Stop();
            backgroundMusic3.Play();
            break;
        }
        
    }
}
