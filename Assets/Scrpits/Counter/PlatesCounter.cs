using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSo plateKitchenObjectSO;

    private float spwanPlateTimer;
    private float spwanPlateTimerMax=4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax=4;

    private void Update()
    {
        spwanPlateTimer += Time.deltaTime;
        if (spwanPlateTimer> spwanPlateTimerMax)
        {
            if ((platesSpawnedAmount < platesSpawnedAmountMax)) {
                platesSpawnedAmount++;  

                OnPlateSpawned?.Invoke(this,  EventArgs.Empty);

                spwanPlateTimer = 0f;

            }

        }    
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        { // player is empty hand
            if (platesSpawnedAmount>0)
            {//at least on plate here
                platesSpawnedAmount--;

                KitchenObject.SpawKitchenObject(plateKitchenObjectSO,player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }



        





}
