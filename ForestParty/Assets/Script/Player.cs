using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static event EventHandler OnAnyPlayerSpawned;
    public static Player LocalInstance {get; private set;}
    void Start()
    {
        
    }
    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            LocalInstance = this;
        }
        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
    }
    void Update()
    {
        
    }
}
