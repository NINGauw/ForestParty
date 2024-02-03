using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class ForestPartyLobby : MonoBehaviour
{
    private Lobby joinedLobby;
    public static ForestPartyLobby Instance { get; private set; }
    private void Awake()
    {
        Instance = this;


        DontDestroyOnLoad(gameObject);
        InitializeUnityAuthentication();
    }

    private async void InitializeUnityAuthentication()
    {
        if(UnityServices.State != ServicesInitializationState.Initialized){
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(Random.Range(0, 10000).ToString());
            await UnityServices.InitializeAsync();

            await AuthenticationService.Instance.SignInAnonymouslyAsync();}
        
    }
    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        try{joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, ForestPartyMultiplayer.MAX_PLAYER, new CreateLobbyOptions{
            IsPrivate = isPrivate,
        });
            ForestPartyMultiplayer.Instance.StartHost();
            Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
        
    }
    public async void QuickJoin()
    {
        try{
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            ForestPartyMultiplayer.Instance.StartClient();
        } catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }

    public async void JoinWithCode(string lobbyCode)
    {
        try{
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

            ForestPartyMultiplayer.Instance.StartClient();
        } catch(LobbyServiceException e)
        { 
            Debug.Log(e);
        }
        
    }
    public Lobby GetLobby()
    {
        return joinedLobby;
    }
}
