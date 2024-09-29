using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance {  get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;

    private float spawRecipeTimer;
    private float spawRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawRecipeTimer -= Time.deltaTime;
        if (spawRecipeTimer<=0)
        {
            spawRecipeTimer = spawRecipeTimerMax;


            if (waitingRecipeSOList.Count<waitingRecipeMax) { 
           RecipeSO waitingrecipeSO =  recipeListSO.recipeSOList[UnityEngine.Random.Range(0,recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingrecipeSO.recipeName);
            waitingRecipeSOList.Add(waitingrecipeSO);

                OnRecipeSpawned?.Invoke(this,EventArgs.Empty);
        }
        }
    }
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i =0; i < waitingRecipeSOList.Count;i++)
        { RecipeSO   waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.kitchenObjectSOList.Count==plateKitchenObject.GetKitchenObjectSoList().Count)
            {//has the same number of ingredients
                bool plateContentMatchesRecipe = true;
                foreach (KitchenObjectSo recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {//Cycling through all ingredients in the Recipe 
                    bool ingredientFound = false;
                    foreach (KitchenObjectSo plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSoList())
                    {//Cycling through all ingredients in the Plate 
                        if (plateKitchenObjectSO==recipeKitchenObjectSO)
                        { //Ingredients matches!
                            ingredientFound= true;
                            break;
                        }
                    }
                    //
                    if (!ingredientFound)
                    { //doesnt match recipe on a plate
                        plateContentMatchesRecipe = false;
                    }
                }
                //
                if (plateContentMatchesRecipe)
                {//Player delivered the correct the recipe
                   // Debug.Log("Player delivered the correct the recipe");
                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this,EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        //Debug.Log("Player didnt delivered the correct the recipe");
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
    return waitingRecipeSOList; 
    }
}
