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

        NPCController.Instance.OnNPCSelectedCounterChanged += Instance_OnNPCSelectedCounterChanged;


    }

    private void Instance_OnNPCSelectedCounterChanged(object sender, NPCController.OnNPCSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCouter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
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
        Debug.Log("Showing visual for counter: " + baseCounter);
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
