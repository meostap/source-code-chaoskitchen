using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSo kitchenObjectSo;
   

   
    public override void Interact(IKitchenObjectParent kitchenObjectParent)
    {
        if (!HasKitchenObject())
        {//no kitchenObject
            if (kitchenObjectParent.HasKitchenObject())
            {//player carring sthing
                kitchenObjectParent.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //not carrying anything
            }

        }
        else {
            // There is a KitchenObject here
            if (kitchenObjectParent.HasKitchenObject()) {
                //player carry sthong
                if (kitchenObjectParent.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject ))
                {//player holding a plate
                   
                  if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo()))
                    {
                        GetKitchenObject().DestroySelf();
                    }

                }
                else
                {//Player carring sthing but not a plate
                    if (GetKitchenObject().TryGetPlate(out  plateKitchenObject))
                    {//Couter holding plate
                        if (plateKitchenObject.TryAddIngredient(kitchenObjectParent.GetKitchenObject().GetKitchenObjectSo()))
                        {
                            kitchenObjectParent.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {//not carrying anything
                GetKitchenObject().SetKitchenObjectParent(kitchenObjectParent);
            }
        }
    }

    
     
}
