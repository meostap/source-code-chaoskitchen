using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;

    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }
    public override void Interact(IKitchenObjectParent kitchenObjectParent)
    {
        if (kitchenObjectParent.HasKitchenObject())
        {
            kitchenObjectParent.GetKitchenObject().DestroySelf();

            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
        
       
    }
}
