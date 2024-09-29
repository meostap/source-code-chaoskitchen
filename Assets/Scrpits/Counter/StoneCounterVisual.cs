using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneCounterVisual : MonoBehaviour
{
    [SerializeField] private StoneCounter stoneCounter;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;

    private void Start()
    {
        stoneCounter.OnStateChanged += StoneCounter_OnStateChanged;
    }

    private void StoneCounter_OnStateChanged(object sender, StoneCounter.OnSateChangedEventArgs e)
    {
        bool showVisual = e.state == StoneCounter.State.Frying || e.state == StoneCounter.State.Fried;
        stoveOnGameObject.SetActive(showVisual);
        particlesGameObject.SetActive(showVisual);
    }
}
