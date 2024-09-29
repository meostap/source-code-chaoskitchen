using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform repiceTemplate;

    private void Awake()
    {
        repiceTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += Instance_OnRecipeCompleted;
        DeliveryManager.Instance.OnRecipeSpawned += Instance_OnRecipeSpawned;
        UpdateVisual();
    }

    private void Instance_OnRecipeSpawned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void Instance_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container)
        { if (child == repiceTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach  (RecipeSO recipeSO in  DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
           Transform recipeTranform =  Instantiate(repiceTemplate,container);
            recipeTranform.gameObject.SetActive(true);
            recipeTranform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);

        }
    }
}
