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
    private int scoreGained;

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
    public void DeliverRecipe(PackageWorkshopObject packageWorkshopObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.workshopObjectSOList.Count == packageWorkshopObject.GetWorkshopObjectSOList().Count)
            {
                //Ma tak¹ sam¹ liczbê czêœci
                bool packageContentsMatchesRecipe = true;
                foreach(WorkshopObjectSO recipeWorkshopObjectSO in waitingRecipeSO.workshopObjectSOList)
                {
                    //Pêtla przez wszystkie czêœci w przepisie 
                    bool partFound = false;
                    foreach (WorkshopObjectSO packageWorkshopObjectSO in packageWorkshopObject.GetWorkshopObjectSOList())
                    {
                        //Pêtla przez wszystkie czêœci w paczce
                        if(packageWorkshopObjectSO == recipeWorkshopObjectSO)
                        {
                            //Czêœci siê zgadzaj¹
                            partFound = true;
                            break;
                        }
                    }
                    if (!partFound)
                    {
                        //Czêœæ z przepisu nie zosta³a znaleziona w paczce
                        packageContentsMatchesRecipe = false;
                    }
                }
                if(packageContentsMatchesRecipe) 
                {
                    //W³aœciwe zamówienie
                    successfulRecipesAmount++;
                    scoreGained += waitingRecipeSO.value * Convert.ToInt32(GameManager.Instance.TimeToMultiplay());
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
    public int GetScoreGained()
    {
        return scoreGained;
    }
}
