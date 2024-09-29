using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSo KitchenObjectSo;
    }


    [SerializeField] private List<KitchenObjectSo> validKitchenObjectSOList;
    private List<KitchenObjectSo> kitchenObjectSoList;

    private void Awake()
    {
        kitchenObjectSoList = new List<KitchenObjectSo>();
    }
    public bool TryAddIgredient(KitchenObjectSo kitchenObjectSo)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSo))
        {//not a vaild ingreddient
            return false;

        }
        if (kitchenObjectSoList.Contains(kitchenObjectSo))
        {
            return false;
        }
        else
        {
            kitchenObjectSoList.Add(kitchenObjectSo);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                KitchenObjectSo = kitchenObjectSo
            });

            return true;
        }
    }
    public List<KitchenObjectSo> GetKitchenObjectSoList()
    {
        return kitchenObjectSoList;
    }

}
