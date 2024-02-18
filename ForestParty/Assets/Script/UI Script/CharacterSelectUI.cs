using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button readyButton;
    [SerializeField] private Text lobbyNameText;
    [SerializeField] private Text lobbyCodeText;


    private void Awake()
    {
        //Khi nhấn nút sẽ gọi phương thức SetPlayerReady();
        readyButton.onClick.AddListener(()=>{
            PlayerSelectReady.Instance.SetPlayerReady();
        });
    }
    private void Start()
    {
        Lobby lobby = ForestPartyLobby.Instance.GetLobby();
        lobbyNameText.text = "Lobby Name: " + lobby.Name;
        lobbyCodeText.text = "Lobby Code: " + lobby.LobbyCode;
    }
}
