using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class AudioSync : NetworkBehaviour
{
    public AudioSource DoNote;
    public AudioSource ReNote;
    public AudioSource MiNote;
    public AudioSource FaNote;
    public AudioSource SolNote;
    public AudioSource LaNote;
    public AudioSource SiNote;
    public AudioSource Do1Note;
    private AudioSource sound;

    private void Start()
    {
        // Chỉ khởi tạo sound trên máy khách
        if (IsClient)
        {
            sound = GetComponent<AudioSource>();
            DoNote = GameObject.Find("DoNote").GetComponent<AudioSource>();
            ReNote = GameObject.Find("ReNote").GetComponent<AudioSource>();
            MiNote = GameObject.Find("MiNote").GetComponent<AudioSource>();
            FaNote = GameObject.Find("FaNote").GetComponent<AudioSource>();
            SolNote = GameObject.Find("SolNote").GetComponent<AudioSource>();
            LaNote = GameObject.Find("LaNote").GetComponent<AudioSource>();
            SiNote = GameObject.Find("SiNote").GetComponent<AudioSource>();
            Do1Note = GameObject.Find("Do1Note").GetComponent<AudioSource>();
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z)){
            if (IsLocalPlayer)
            {
            RequestPlaySoundServerRpc(0);
            }
        }
        if(Input.GetKeyDown(KeyCode.X)){
            if (IsLocalPlayer)
            {
            RequestPlaySoundServerRpc(1);
            }
        }
        if(Input.GetKeyDown(KeyCode.C)){
            if (IsLocalPlayer)
            {
            RequestPlaySoundServerRpc(2);
            }
        }
        if(Input.GetKeyDown(KeyCode.V)){
            if (IsLocalPlayer)
            {
            RequestPlaySoundServerRpc(3);
            }
        }
        if(Input.GetKeyDown(KeyCode.B)){
            if (IsLocalPlayer)
            {
            RequestPlaySoundServerRpc(4);
            }
        }
        if(Input.GetKeyDown(KeyCode.N)){
            if (IsLocalPlayer)
            {
            RequestPlaySoundServerRpc(5);
            }
        }
        if(Input.GetKeyDown(KeyCode.M)){
            if (IsLocalPlayer)
            {
            RequestPlaySoundServerRpc(6);
            }
        }
        if(Input.GetKeyDown(KeyCode.Comma)){
            if (IsLocalPlayer)
            {
            RequestPlaySoundServerRpc(7);
            }
        }
    }
    [ServerRpc]
    private void RequestPlaySoundServerRpc(int noteIndex)
    {
        PlaySoundClientRpc(noteIndex);
    }
    [ClientRpc]
    private void PlaySoundClientRpc(int noteIndex)
    {
        switch (noteIndex){
            case 0:
            DoNote.PlayOneShot(DoNote.clip);
            break;
            case 1:
            ReNote.PlayOneShot(ReNote.clip);
            break;
            case 2:
            MiNote.PlayOneShot(MiNote.clip);
            break;
            case 3:
            FaNote.PlayOneShot(FaNote.clip);
            break;
            case 4:
            SolNote.PlayOneShot(SolNote.clip);
            break;
            case 5:
            LaNote.PlayOneShot(LaNote.clip);
            break;
            case 6:
            SiNote.PlayOneShot(SiNote.clip);
            break;
            case 7:
            Do1Note.PlayOneShot(Do1Note.clip);
            break;
        }
        
    }    
}
