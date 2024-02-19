using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField]private int playerIndex;
    private void Start()
    {
        ForestPartyMultiplayer.Instance.OnPlayerNetworkChanged += ForestPartyMultiplayer_OnplayerNetworkChanged;
        //Phải có UpdatePlayer ở start bởi vì Khi host tạo phòng sẽ không kích hoạt sự kiện OnPlayerNetworkChanged hay đúng hơn là sự kiện OnListChanged của kiểu dữ liệu NetworkList
        UpdatePlayer();
    }

    private void ForestPartyMultiplayer_OnplayerNetworkChanged(object sender, System.EventArgs e)
    {
        //Debug.Log(ForestPartyMultiplayer.Instance.IsPlayerIndexConnected(playerIndex));
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        
        if(ForestPartyMultiplayer.Instance.IsPlayerIndexConnected(playerIndex)){
            Show();
        }
        else{
            Hide();
        }
    }
    private void Show(){
        gameObject.SetActive(true);
    }
    
    private void Hide(){
        gameObject.SetActive(false);
    }
}
