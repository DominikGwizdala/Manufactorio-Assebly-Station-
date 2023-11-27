using System;
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
    //[SerializeField] private Transform timer;
    [SerializeField] private Image timerImage;
    private RecipeSO localRecipeSO;

    private float timerIndex = 0;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    public void SetRecipeSO(RecipeSO recipeSO)
    {
        localRecipeSO = recipeSO;

        recipeNameText.text = recipeSO.recipeName;
        
        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (WorkshopObjectSO workshopObjectSO in recipeSO.workshopObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = workshopObjectSO.sprite;
        }
    }

    private void Update()
    {
        float localTimer = 0;
        localTimer += Time.deltaTime;
        timerImage.fillAmount = 1 - (localTimer / localRecipeSO.value);
    }
}
