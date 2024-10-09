using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelUIManager : MonoBehaviour
{
    [SerializeField] private Button nextlevelButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionButton;



    private void Awake()
    {

        gameObject.SetActive(false);
        nextlevelButton.onClick.AddListener(() =>
        {
            DeliveryManager.Instance.TransitionToLevel2();  // Gọi hàm StartLevel2 từ DeliveryManager
            gameObject.SetActive(false);  // Tắt UI sau khi chuyển màn
            Time.timeScale = 1f;
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            Debug.Log("1");
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        optionButton.onClick.AddListener(() =>

        {
            Debug.Log("2");
            OptionsUI.Instance.Show();
        });
    }
    public void ShowNextLevelUI()
    {

        gameObject.SetActive(true);
        Time.timeScale = 0f;  // Tạm dừng game
    }



    
}
