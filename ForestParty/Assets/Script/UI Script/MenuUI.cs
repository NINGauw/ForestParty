using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : NetworkBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainmenuButton;
    
    private void Awake()
    {
        resumeButton.onClick.AddListener(()=>{
            ManageMenuUI.Instance.Hide();
        });
        mainmenuButton.onClick.AddListener(()=>{
            Application.Quit();
        });
    }
}
