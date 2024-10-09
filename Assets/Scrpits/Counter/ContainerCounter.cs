using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSo kitchenObjectSo;

    public void Interact(NPCController NPC)
    {
        Debug.Log("NPC interacting with ContainerCounter");

        if (!NPC.HasKitchenObject())
        {
            Debug.Log("NPC picked up item from ContainerCounter");
            KitchenObject.SpawKitchenObject(kitchenObjectSo, NPC);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.Log("NPC already has an item, cannot pick up another.");
        }


    }

    public override void Interact(IKitchenObjectParent kitchenObjectParent)
    {
        if (!kitchenObjectParent.HasKitchenObject()) {


            KitchenObject.SpawKitchenObject(kitchenObjectSo,kitchenObjectParent);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }


    }
   


}
