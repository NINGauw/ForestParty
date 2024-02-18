using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.EventSystems;

public class ForestPartyLobby : MonoBehaviour
{
    private const string KEY_RELAY_CODE = "RelayJoinCode";
    private Lobby joinedLobby;
    public static ForestPartyLobby Instance { get; private set; }
    //Tạo 1 delegate để gọi kiểu EventHandler nhưng có tham số là OnlobbyListChangedEventArgs
    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
    public class OnLobbyListChangedEventArgs : EventArgs {
        public List<Lobby> lobbyList;
    }

    private float listLobbiesTimer;
    private void Awake()
    {
        Instance = this;


        DontDestroyOnLoad(gameObject);
        InitializeUnityAuthentication();
    }
    private void Update()
    {
        HandlePeriodicListLobbies();
    }

    private void HandlePeriodicListLobbies()
    {
        if(joinedLobby == null && AuthenticationService.Instance.IsSignedIn){
            listLobbiesTimer -= Time.deltaTime;
            if(listLobbiesTimer <= 0f){
                float listLobbiesTimerMax = 3f;
                listLobbiesTimer = listLobbiesTimerMax;
                ListLobbies();
            } 
        }
    }

    private async void InitializeUnityAuthentication()
    {
        if(UnityServices.State != ServicesInitializationState.Initialized){
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(UnityEngine.Random.Range(0, 10000).ToString());
            await UnityServices.InitializeAsync();

            await AuthenticationService.Instance.SignInAnonymouslyAsync();}
        
    }
    private async void ListLobbies()
    {
        try{
        QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions{
            Filters = new List<QueryFilter>{
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
            }
        };
        QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
        
        OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs{
            lobbyList = queryResponse.Results
        });
        } catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }
    //Hàm phân bổ Relay
    private async Task<Allocation> AllocateRelay()
    {
        try{
          Allocation allocation = await RelayService.Instance.CreateAllocationAsync(ForestPartyMultiplayer.MAX_PLAYER - 1);
          return allocation;
        }   catch(RelayServiceException e){
            Debug.Log(e);
            return default;
        }
        }
    //Hàm lấy code từ relay
    private async Task<string> GetRelayJoinCode(Allocation allocation)
    {
        try{string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        return relayJoinCode;
        }   catch(RelayServiceException e){
            Debug.Log(e);
            return default;
        }
    }
    private async Task<JoinAllocation> JoinRelay(string joinCode){
        try{
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            return joinAllocation;
        }   catch(RelayServiceException e){
            Debug.Log(e);
            return default;
        }
        
    }
    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        try{joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, ForestPartyMultiplayer.MAX_PLAYER, new CreateLobbyOptions{
            IsPrivate = isPrivate,
        });
            //Note
            Allocation allocation = await AllocateRelay();
            string relayJoinCode = await GetRelayJoinCode(allocation);
            
            await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions{
                Data = new Dictionary<string, DataObject>{
                    {KEY_RELAY_CODE, new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode)}
                }
            });
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));   

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

            string relayJoinCode = joinedLobby.Data[KEY_RELAY_CODE].Value;
            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
            
            ForestPartyMultiplayer.Instance.StartClient();
        } catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }

    public async void JoinWithCode(string lobbyCode)
    {
        try{
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

            string relayJoinCode = joinedLobby.Data[KEY_RELAY_CODE].Value;
            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            ForestPartyMultiplayer.Instance.StartClient();
        } catch(LobbyServiceException e)
        { 
            Debug.Log(e);
        }
        
    }
    public async void JoinWithID(string lobbyId)
    {
        try{
            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);

            string relayJoinCode = joinedLobby.Data[KEY_RELAY_CODE].Value;
            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

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
