using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;  
    public event EventHandler OnRecipeFailed;
    [SerializeField] private RecipeSO firstOrderRecipe;

    private bool isFirstOrderSpawned = false;

    private int playerGold = 0; // Vàng của người chơi
    private int goldToNextLevel = 8; // Số vàng cần để qua cấp độ mới


    [SerializeField] private Slider goldBarSlider;
    [SerializeField] private NextLevelUIManager nextLevelUIManager;

    public static DeliveryManager Instance {  get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 6f;
    private int waitingRecipeMax = 4;
    private int successfullRecipeAmount;
    private void Awake()
    {
        Instance = this;
        goldBarSlider.interactable = false;
        waitingRecipeSOList = new List<RecipeSO>();
        goldBarSlider.maxValue = goldToNextLevel;
        goldBarSlider.value = playerGold;
    }
    private void Update()
    {
        HandleRecipeSpawning();
        if (playerGold >= goldToNextLevel)
        {
            TransitionToLevel2();  // Chuyển sang Level 2
        }
    }

    // Hàm sinh công thức
    private void HandleRecipeSpawning()
    {
        spawnRecipeTimer -= Time.deltaTime;

        if (spawnRecipeTimer <= 0)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            // Kiểm tra nếu là đơn hàng đầu tiên
            if (!isFirstOrderSpawned)
            {
                waitingRecipeSOList.Add(firstOrderRecipe);  // Thêm công thức đầu tiên
                isFirstOrderSpawned = true;
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
                return;
            }

            // Kiểm tra nếu đã đủ vàng để chuyển sang Level 2
            if (playerGold < goldToNextLevel)
            {
                SpawnRecipesForLevel1();  // Sinh công thức cho Level 1
            }
            else
            {
                SpawnRecipesForLevel2();  // Sinh công thức cho Level 2
            }
        }
    }

    // Sinh công thức cho Level 1
    private void SpawnRecipesForLevel1()
    {
        if (waitingRecipeSOList.Count < waitingRecipeMax)
        {
            List<RecipeSO> filteredRecipeList = recipeListSO.recipeSOList.FindAll(recipe => recipe.kitchenObjectSOList.Count <= 3);

            if (filteredRecipeList.Count > 0)
            {
                RecipeSO waitingRecipeSO = filteredRecipeList[UnityEngine.Random.Range(0, filteredRecipeList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    // Sinh công thức cho Level 2
    private void SpawnRecipesForLevel2()
    {
        if (waitingRecipeSOList.Count < waitingRecipeMax)
        {
            RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
            waitingRecipeSOList.Add(waitingRecipeSO);
            OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
        }
    }

    // Hàm xử lý khi giao công thức
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSoList().Count)
            {
                bool plateContentMatchesRecipe = true;

                foreach (KitchenObjectSo recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;

                    foreach (KitchenObjectSo plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSoList())
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        plateContentMatchesRecipe = false;
                        break;
                    }
                }

                if (plateContentMatchesRecipe)
                {
                    successfullRecipeAmount++;
                    waitingRecipeSOList.RemoveAt(i);

                    // Thêm vàng cho người chơi
                    playerGold += waitingRecipeSO.goldReward;
                    goldBarSlider.value = playerGold;

                    // Kiểm tra nếu đủ vàng để chuyển sang Level 2
                    if (playerGold >= goldToNextLevel)
                    {
                        TransitionToLevel2();  // Chuyển sang Level 2
                    }

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }

        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    // Hàm chuyển sang Level 2
    public void TransitionToLevel2()
    {
        // Hiển thị UI để người chơi chọn tiếp tục
        ShowNextLevelUI();

        // Thiết lập lại thanh vàng và yêu cầu số vàng cho cấp độ tiếp theo
        goldToNextLevel = 30;  // Số vàng cần cho cấp độ 3
        playerGold = 0;  // Reset số vàng
        goldBarSlider.maxValue = goldToNextLevel;
        goldBarSlider.value = playerGold;

        // Tăng số lượng công thức tối đa trong Level 2
        waitingRecipeMax = 4;

        // Đảm bảo đủ số lượng công thức cho Level 2
        while (waitingRecipeSOList.Count < waitingRecipeMax)
        {
            RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
            waitingRecipeSOList.Add(waitingRecipeSO);
        }

        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
    return waitingRecipeSOList; 
    }
    public int GetSuccessfullRecipeAmount()
    {
        return successfullRecipeAmount;
    }

    private void ShowNextLevelUI()
    {
        nextLevelUIManager.ShowNextLevelUI(); // Gọi hàm hiển thị UI từ NextLevelUIManager
    }
}
