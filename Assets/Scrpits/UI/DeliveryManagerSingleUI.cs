using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipeSO)
    {
        recipeNameText.text = recipeSO.recipeName;

        foreach (Transform chill in iconContainer)
        { if (chill == iconTemplate) continue;
        Destroy(chill.gameObject);

        }

        foreach (KitchenObjectSo kitchenObjectSo in recipeSO.kitchenObjectSOList)
        {
           Transform iconTransform = Instantiate(iconTemplate,iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSo.sprite;
        }
    }
}
