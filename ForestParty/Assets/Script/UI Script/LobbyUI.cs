using System.Collections;
using System.Collections.Generic;
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
    }
}
