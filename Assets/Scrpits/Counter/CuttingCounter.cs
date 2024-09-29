using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CuttingCounter : BaseCounter ,IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArg> OnProgressChanged;

    public static event EventHandler OnAnyCut;
   
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgrees;    
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {//no kitchenObject
            if (player.HasKitchenObject())
            {//player carring sthing
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo())) { 
                player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgrees = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArg
                    {
                        progressNomalized =(float) cuttingProgrees/ cuttingRecipeSO.cuttingProgressMax
                    });

                }
            }
            else
            {
                //not carrying anything
            }

        }
        else
        {
            // There is a KitchenObject here
            if (player.HasKitchenObject())
            {
                //player carry sthing
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {//player holding a plate

                    if (plateKitchenObject.TryAddIgredient(GetKitchenObject().GetKitchenObjectSo()))
                    {
                        GetKitchenObject().DestroySelf();
                    }

                }
            }
            else
            {//not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSo()))
        {
            //have sthing on couter and it can be cut
            cuttingProgrees++;
            OnCut?.Invoke(this,EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());


            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArg
            {
                progressNomalized = (float)cuttingProgrees / cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgrees >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSo outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSo());

                // Xóa KitchenObject hiện tại
                GetKitchenObject().DestroySelf();

                // Tạo KitchenObject mới dựa trên KitchenObjectSO đầu ra
                KitchenObject.SpawKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSo inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSo GetOutputForInput(KitchenObjectSo inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSo inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
