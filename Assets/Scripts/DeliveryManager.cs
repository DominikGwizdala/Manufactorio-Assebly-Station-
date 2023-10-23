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

    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int successfulRecipesAmount;

    private void Awake()
    {
        Instance = this;

        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <=0f) 
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (GameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public void DeliverRecipe(PlateWorkshopObject plateWorkshopObject)
    {
        for(int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.workshopObjectSOList.Count == plateWorkshopObject.GetWorkshopObjectSOList().Count)
            {
                //Ma tak¹ sam¹ liczbê sk³adników
                bool plateContentsMatchesRecipe = true;
                foreach(WorkshopObjectSO recipeWorkshopObjectSO in waitingRecipeSO.workshopObjectSOList)
                {
                    //Pêtla przez wszystkie sk³adniki w Przepisie 
                    bool ingredientFound = false;
                    foreach (WorkshopObjectSO plateWorkshopObjectSO in plateWorkshopObject.GetWorkshopObjectSOList())
                    {
                        //Pêtla przez wszystkie skadniki na talerzu\
                        if(plateWorkshopObjectSO == recipeWorkshopObjectSO)
                        {
                            //Sk³adniki siê zgadzaj¹
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        //Sk³adnik przepisu nie zosta³ znaleziony na talerzu
                        plateContentsMatchesRecipe = false;
                    }
                }
                if(plateContentsMatchesRecipe) 
                {
                    //W³aœciwe zamówienie
                    successfulRecipesAmount++;
                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }   
            }
        }
        //Brak zamówienia
        //Gracz dostarczy³ z³e zamówienie
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
    public int GetSuccessfulRecipesAmount() {
        return successfulRecipesAmount;
    }
}
