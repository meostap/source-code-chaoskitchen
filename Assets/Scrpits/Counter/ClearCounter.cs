using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSo kitchenObjectSo;
   

   
    public override void  Interact(Player player)
    {
        if (!HasKitchenObject())
        {//no kitchenObject
            if (player.HasKitchenObject())
            {//player carring sthing
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //not carrying anything
            }

        }
        else {
            // There is a KitchenObject here
            if (player.HasKitchenObject()) {
                //player carry sthong
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject ))
                {//player holding a plate
                   
                  if(plateKitchenObject.TryAddIgredient(GetKitchenObject().GetKitchenObjectSo()))
                    {
                        GetKitchenObject().DestroySelf();
                    }

                }
                else
                {//Player carring sthing but not a plate
                    if (GetKitchenObject().TryGetPlate(out  plateKitchenObject))
                    {//Couter holding plate
                        if (plateKitchenObject.TryAddIgredient(player.GetKitchenObject().GetKitchenObjectSo()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {//not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    
     
}
