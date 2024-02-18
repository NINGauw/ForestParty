using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListSingleUI : MonoBehaviour
{
    [SerializeField]private Text lobbyNameText;
    private Lobby lobby;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(()=>{
            ForestPartyLobby.Instance.JoinWithID(lobby.Id);
        });
    }
    public void SetLobby(Lobby lobby){
        this.lobby = lobby;
        lobbyNameText.text = lobby.Name;
    }
}
