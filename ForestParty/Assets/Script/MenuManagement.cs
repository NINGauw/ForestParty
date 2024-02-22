using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManagement : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private TutorialUI tutorialUI;
    private void Awake()
    {
        startGameButton.onClick.AddListener(()=>{
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
        tutorialButton.onClick.AddListener(()=>{
            tutorialUI.Show();
        });
    }

}
