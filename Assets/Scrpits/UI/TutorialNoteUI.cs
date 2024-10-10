using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialNoteUI : MonoBehaviour
{
    [SerializeField] private GameObject TutorialNotePanel;  // UI Panel chính
    [SerializeField] private GameObject backgroundPanel;
    [SerializeField] private Button GuideBtn;


    private void Start()
    {
        // Ban đầu ẩn cả UI Panel và Background Panel
        TutorialNotePanel.SetActive(false);
        backgroundPanel.SetActive(false);
        GuideBtn.onClick.AddListener(ShowUIPanel);
        backgroundPanel.GetComponent<Button>().onClick.AddListener(HideUIPanel);
        TutorialNotePanel.GetComponent<Button>().onClick.AddListener(HideUIPanel);
    }



    // Hàm để mở UI Panel khi nhấn nút
    public void ShowUIPanel()
    {
        TutorialNotePanel.SetActive(true);
        backgroundPanel.SetActive(true);  // Hiển thị Panel nền để phát hiện click
    }

    // Hàm để ẩn UI Panel khi nhấn ra ngoài
    public void HideUIPanel()
    {
        TutorialNotePanel.SetActive(false);
        backgroundPanel.SetActive(false);
    }
}
