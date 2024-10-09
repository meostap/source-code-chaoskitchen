using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlayHere;
    [SerializeField] private Transform couterTopPoint;
    [SerializeField] private KitchenObjectSo kitchenObjectSO;

    private KitchenObject kitchenObject;
    public virtual void Interact(IKitchenObjectParent kitchenObjectParent)
    {
        Debug.Log("BaseCounter.Interact();");
    }
    public static void ResetStaticData()
    {
        OnAnyObjectPlayHere = null;
    }

    public virtual void InteractAlternate(Player player)
    {
        //Debug.Log("BaseCounter.InteractAlternate();");
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return couterTopPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {

            OnAnyObjectPlayHere?.Invoke(this, EventArgs.Empty);
        }
    }
   
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void CleaKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject() { return kitchenObject != null; }

  
    /*public override void Interact(IKitchenObjectParent kitchenObjectParent)
    {
        if (!kitchenObjectParent.HasKitchenObject())
        {
            // Tạo vật phẩm và đặt vào tay NPC hoặc người chơi
            KitchenObject.SpawKitchenObject(kitchenObjectSO, kitchenObjectParent);
        }
    }*/
}
