using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button joinCodeButton;
    [SerializeField] private InputField joinCodeInputField;
    [SerializeField] private LobbyCreateUI lobbyCreateUI;
    [SerializeField] private Transform lobbyContainer;
    [SerializeField] private Transform lobbyTemplate;


    private void Awake(){
        mainMenuButton.onClick.AddListener(()=>{
            Loader.Load(Loader.Scene.StartScene);
        });
        createLobbyButton.onClick.AddListener(()=>{
            lobbyCreateUI.Show();
        });
        quickJoinButton.onClick.AddListener(()=>{
            ForestPartyLobby.Instance.QuickJoin();
        });
        joinCodeButton.onClick.AddListener(()=>{
            ForestPartyLobby.Instance.JoinWithCode(joinCodeInputField.text);
        });
        ForestPartyLobby.Instance.OnLobbyListChanged += ForestPartyLobby_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());

        lobbyTemplate.gameObject.SetActive(false);
    }

    private void ForestPartyLobby_OnLobbyListChanged(object sender, ForestPartyLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in lobbyContainer){
            if(child == lobbyTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (Lobby lobby in lobbyList){
            Transform lobbyTranform = Instantiate(lobbyTemplate, lobbyContainer);
            lobbyTranform.gameObject.SetActive(true);
            lobbyTranform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
        }
    }
}
