using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChopBtnUI : MonoBehaviour
{
    [SerializeField] private Button chopButton;
    [SerializeField] private Player player; // Tham chiếu đến script của người chơi

    private void Start()
    {
        // Thêm listener vào nút để khi nhấn nút sẽ gọi hàm ChopButton
        chopButton.onClick.AddListener(ChopButton);
    }

    // Hàm xử lý tương tác với tất cả Counter khi nhấn nút
    private void ChopButton()
    {
        if (player != null)
        {
            player.ChoppingButton();
        }
    }
}
