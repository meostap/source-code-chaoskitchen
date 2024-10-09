using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpBtnUI : MonoBehaviour
{
    [SerializeField] private Button interactionButton;
    [SerializeField] private Player player; // Tham chiếu đến script của người chơi

    private void Start()
    {
        // Thêm listener vào nút để khi nhấn nút sẽ gọi hàm ChopButton
        interactionButton.onClick.AddListener(PickupButton);
    }

    // Hàm xử lý tương tác với tất cả Counter khi nhấn nút
    private void PickupButton()
    {
        if (player != null)
        {
            player.PickupButton();
        }
    }
}
