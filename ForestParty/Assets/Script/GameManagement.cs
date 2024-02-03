using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : NetworkBehaviour
{
    public static GameManagement Instance {get; private set;}
    [SerializeField] private Transform playerPrefab;

    private void Awake()
    {
        Instance = this;
    }
    //OnNetworkSpawn là hàm sẽ được gọi khi 1 networkobject được tạo
    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            //NetworkManager.Singleton.SceneManager này khác với SceneManager của UnityEngine.SceneManagement
            //Nếu đang có sever thì sẽ chạy thêm SceneManager_OnLoadEventCompleted khi Scene được load hoàn tất ( biết được do OnLoadEventCompleted)
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    //Hàm này đùng để spawn các player prefabs 
    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        //Với mỗi clientID đã connect vào trong network thì sẽ khởi tạo 1 playerPrefab
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        { 
            //Instantiate(ObjectPrefab, position, rotation)
            Transform playerTransform = Instantiate(playerPrefab);//Instantiate dùng để tạo 1 bản sao của 1 đối tượng
            //SpawnAsPlayerObject là 1 phương thức để spawn 1 đối tượng vào môi trường mạng và gán vào đối tượng của người chơi cụ thể trên mạng
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }
}
