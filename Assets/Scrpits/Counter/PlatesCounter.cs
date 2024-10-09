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
            if ((KitchenGameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)) {
                platesSpawnedAmount++;  

                OnPlateSpawned?.Invoke(this,  EventArgs.Empty);

                spwanPlateTimer = 0f;

            }

        }    
    }

    public override void Interact(IKitchenObjectParent kitchenObjectParent)
    {
        if (!kitchenObjectParent.HasKitchenObject())
        {
            // Nếu NPC hoặc Player đang không cầm vật phẩm nào
            if (platesSpawnedAmount > 0)
            {
                // Nếu có ít nhất một cái đĩa ở đây
                platesSpawnedAmount--;

                // Spawn đĩa và gán nó cho NPC hoặc Player
                KitchenObject.SpawKitchenObject(plateKitchenObjectSO, kitchenObjectParent);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
