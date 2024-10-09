using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPChatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI chatText;

    private void Start()
    {
        gameObject.SetActive(false); // Đảm bảo UI ẩn khi bắt đầu
    }
    public void DisplayBurgerRecipe()
    {
        chatText.text = "And that is how to make \n A Burger";
    }
}
