using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check1Script : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collider2D){
        if(collider2D.CompareTag("Player"))
        BackGroundAudioManagement.Instance.ChangeBackground(0);
    }
}
