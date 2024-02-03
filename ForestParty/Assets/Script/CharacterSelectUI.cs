using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button readyButton;

    private void Awake()
    {
        //Khi nhấn nút sẽ gọi phương thức SetPlayerReady();
        readyButton.onClick.AddListener(()=>{
            PlayerSelectReady.Instance.SetPlayerReady();
        });
    }
}
