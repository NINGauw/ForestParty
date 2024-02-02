using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForestPartyMultiplayer : NetworkBehaviour
{
    public static ForestPartyMultiplayer Instance {get; private set;}
    public event EventHandler OnTryingToJoin;
    public event EventHandler OnFailedToJoin;

    private const int MAX_PLAYER = 4;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if(SceneManager.GetActiveScene().name != Loader.Scene.CharacterSelectScene.ToString())
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started";
            return;
        }
        if(NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game full";
            return;
        }
        connectionApprovalResponse.Approved = true;
        //connectionApprovalResponse.CreatePlayerObject = true;
    }

    public void StartClient()
    {
        OnTryingToJoin?.Invoke(this, EventArgs.Empty);

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartClient();
        
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        OnFailedToJoin?.Invoke(this, EventArgs.Empty);
    }
}
