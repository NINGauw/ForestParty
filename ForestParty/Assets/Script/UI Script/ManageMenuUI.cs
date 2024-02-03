using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageMenuUI : MonoBehaviour
{
    public static ManageMenuUI Instance {get; private set; }
    [SerializeField] private Button openMenuButton;
    [SerializeField] private GameObject menu;

    // Update is called once per frame
    private void Awake()
    {
        Instance = this;
        openMenuButton.onClick.AddListener(Show);
    }
    private void Start()
    {
        Hide();
    }
    public void Show()
    {
        menu.SetActive(true);
    }
    public void Hide()
    {
        menu.SetActive(false);
    }
}
