using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchAreaUI : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Screen clicked!");

        if (!KitchenGameManager.Instance.IsGamePlaying() && !KitchenGameManager.Instance.IsCountdownToStartAcitve())
        {
            KitchenGameManager.Instance.StartGame();
            Debug.Log("Game started");
        }

        if (TutorialUI.Instance != null)
        {
            TutorialUI.Instance.Hide();
            Debug.Log("Tutorial hidden");
        }
    }
}
