using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForestPartyMultiplayer : NetworkBehaviour
{
    public static ForestPartyMultiplayer Instance {get; private set;}
    public event EventHandler OnTryingToJoin;//Tạo 1 sự kiện khi người chơi vào game
    public event EventHandler OnFailedToJoin;//Tạo 1 sự kiện khi người chơi vào game thất bại
    public event EventHandler OnPlayerNetworkChanged;//Tạo sự kiện khi số lượng người chơi thay đổi

    public const int MAX_PLAYER = 4;
    //Biến lưu danh sách người chơi tham gia
    private NetworkList<PlayerData> playerDataNetworkList;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        playerDataNetworkList = new NetworkList<PlayerData>();
        //thêm sự kiện OnPlayerNetworkChanged vào sự kiện OnListChanged của biến playerDataNetworkList
        playerDataNetworkList.OnListChanged += playerDataNetworkList_OnlistChanged;
    }

    private void playerDataNetworkList_OnlistChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        Debug.Log("call success");
        OnPlayerNetworkChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartHost()
    {
        //ConnectApprovalCallback dùng để xử lý phê duyệt kết nối và đang thêm phương thức NetworkManager_ConnectionApprovalCallback vào các phương thức sẽ gọi khi 1 client kết nối đến sever
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnCLientConnectedCallback;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_OnCLientConnectedCallback(ulong clientIdMulti)
    {
        playerDataNetworkList.Add(new PlayerData{
            clientId = clientIdMulti,
        });
    }

    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {  
        //Nếu màn hiện tại không phải màn hình chọn nhân vật, thì phản hồi phê duyệt kết nối là false, ngăn không cho truy cập vào phòng
        if(SceneManager.GetActiveScene().name != Loader.Scene.CharacterSelectScene.ToString())
        {
            Debug.Log("NotReadyScene");
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started";
            return;
        }
        //Kiểm duyệt số lượng người tham gia 1 host
        if(NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER)
        {
            Debug.Log("Full");
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game full";
            return;
        }
        else
        connectionApprovalResponse.Approved = true;
    }

    public void StartClient()
    {
        //Kiểm tra xem người dùng có đang tham gia không nếu không thì không thực hiện lệnh này
        OnTryingToJoin?.Invoke(this, EventArgs.Empty);

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartClient();
        
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        OnFailedToJoin?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerIndexConnected(int playerIndex)
    {
        return playerIndex < playerDataNetworkList.Count;
    }
}
