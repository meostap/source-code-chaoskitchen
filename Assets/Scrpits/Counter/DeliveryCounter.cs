using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public override void Interact(IKitchenObjectParent kitchenObjectParent)
    {
        if (kitchenObjectParent.HasKitchenObject())
        {
            if (kitchenObjectParent.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {//only accepts Plates

                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                kitchenObjectParent.GetKitchenObject().DestroySelf();
            }
        }
        
    }

}
    