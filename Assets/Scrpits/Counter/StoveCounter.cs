using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter,IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArg> OnProgressChanged;
    public event EventHandler<OnSateChangedEventArgs> OnStateChanged;
    public class OnSateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }
    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArg
                    {
                        progressNomalized = fryingTimer / fryingRecipeSO.fryingTimeMax
                    });

                    if (fryingTimer > fryingRecipeSO.fryingTimeMax)
                {//Fried  
                    GetKitchenObject().DestroySelf();

                    KitchenObject.SpawKitchenObject(fryingRecipeSO.output, this);
                        Debug.Log("Object Fried");

                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());

                        OnStateChanged?.Invoke(this,new OnSateChangedEventArgs
                        {
                            state = state,
                        });
                    }
                break;
            case State.Fried:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArg
                    {
                        progressNomalized = burningTimer / burningRecipeSO.burningTimeMax
                    });

                    if (burningTimer > burningRecipeSO.burningTimeMax)
                    {//Fried  
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawKitchenObject(burningRecipeSO.output, this);
                        Debug.Log("Object Burned");
                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnSateChangedEventArgs
                        {
                            state = state,
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArg
                        {
                            progressNomalized = 0f
                        });
                    }
                    
                    break;
            case State.Burned:
                break;

        }    Debug.Log(state);
        
           
            Debug.Log(fryingTimer);
        }
    }
    public override void Interact(IKitchenObjectParent kitchenObjectParent)
    {
        if (!HasKitchenObject())
        {//no kitchenObject
            if (kitchenObjectParent.HasKitchenObject())
            {//player carring sthing
                if (HasRecipeWithInput( kitchenObjectParent.GetKitchenObject().GetKitchenObjectSo()))
                {
                    //has sthing canbe fried
                    kitchenObjectParent.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());
                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnSateChangedEventArgs
                    {
                        state = state,
                    });
                    OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArg
                    {
                        progressNomalized = fryingTimer/ fryingRecipeSO.fryingTimeMax
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
            if (kitchenObjectParent.HasKitchenObject())
            {
                //player carry sthong
                if (kitchenObjectParent.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {//player holding a plate

                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo()))
                    {
                        GetKitchenObject().DestroySelf();

                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnSateChangedEventArgs
                        {
                            state = state,
                        });


                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArg
                        {
                            progressNomalized = 0f
                        });
                    }

                }
            }
            else
            {//not carrying anything
                GetKitchenObject().SetKitchenObjectParent(kitchenObjectParent);


                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnSateChangedEventArgs
                {
                    state = state,
                });


                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArg
                {
                    progressNomalized = 0f
                });
            }
        }
    }
    private bool HasRecipeWithInput(KitchenObjectSo inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSo GetOutputForInput(KitchenObjectSo inputKitchenObjectSO)
    {
        FryingRecipeSO cuttingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSo inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSo inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
    public bool IsFried()
    {
        return state== State.Fried;
    
    }
}
