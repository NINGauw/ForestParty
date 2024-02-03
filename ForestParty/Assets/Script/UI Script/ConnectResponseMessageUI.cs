using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectResponseMessageUI : MonoBehaviour
{
    [SerializeField] private Text messageText;
    [SerializeField] private Button closeButton;
    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }
    private void Start()
    {
        ForestPartyMultiplayer.Instance.OnFailedToJoin += ForestPartyMultiplayer_OnFailedToJoin;
        Hide();
    }

    private void ForestPartyMultiplayer_OnFailedToJoin(object sender, EventArgs e)
    {
        Show();
        messageText.text = NetworkManager.Singleton.DisconnectReason;
        //Nếu kết nối lâu không được thì sẽ báo lỗi
        //Do NetworkManager set Max connect attemp là 6 nên sau 6 lần thử thì sẽ phản hồi OnClientDisconnectCallback
        if(messageText.text == "")
        {
            messageText.text = "Failed to connect";
        }
    }

    private void Show(){
        gameObject.SetActive(true);
    }
    private void Hide(){
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        ForestPartyMultiplayer.Instance.OnFailedToJoin -= ForestPartyMultiplayer_OnFailedToJoin;
    }
}
