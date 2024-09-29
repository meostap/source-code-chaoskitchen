using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCouterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] GameObject[] visualGameObjectArray ;
    void Start()
    {
        Player.Instace.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCouter == baseCounter) {
            Show();
        }
        else
        {
            Hide();
        }
    }
    
    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }
    private void Hide()
    {

        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }



}
