using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSelectReady : NetworkBehaviour
{
    public static PlayerSelectReady Instance {get; private set;}
    //Tạo 1 danh sách các client tham gia
    private Dictionary<ulong, bool> playerSelectReady;
    private void Awake()
    {
        Instance = this;
        playerSelectReady = new Dictionary<ulong, bool>();
    }
    //Dùng phương thức của Client để gọi phương thức truyền lên Sever
    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }
    //Cho phép client gọi phương thức này mà không phải chủ sở hữu, làm vậy để 1 client có thể thực hiện 1 hành động chung đến toàn game.
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        //Đặt thuộc tính ready của client nhấn nút ready là true
        playerSelectReady[serverRpcParams.Receive.SenderClientId] = true;

        //Để duyệt các client tham gia đều ready
        bool allClientReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(!playerSelectReady.ContainsKey(clientId) || !playerSelectReady[clientId])
            {
                //This player is not ready
                allClientReady = false;
                break;
            }
        }
        //Nếu tất cả client cùng ready thì chuyển scene sang gamescene
        if (allClientReady)
        {
            Loader.LoadNetwork(Loader.Scene.GameScene);
        }
    }
}
