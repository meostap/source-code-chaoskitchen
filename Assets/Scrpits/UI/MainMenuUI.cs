using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() => //as private void sthing like PlayClick()
        {
            Loader.Load(Loader.Scene.GameScene);
        });

        quitButton.onClick.AddListener(() => //as private void sthing like PlayClick()
        {
            Application.Quit();
        });

        Time.timeScale = 1f;
    }




    //private void PlayClick()
    

    
}
