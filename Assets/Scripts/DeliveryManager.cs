using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;

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

            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO.recipeName);
                waitingRecipeSOList.Add(waitingRecipeSO);
            }
        }
    }
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for(int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                //Ma tak� sam� liczb� sk�adnik�w
                bool plateContentsMatchesRecipe = true;
                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    //P�tla przez wszystkie sk�adniki w Przepisie 
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        //P�tla przez wszystkie skadniki na talerzu\
                        if(plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            //Sk�adniki si� zgadzaj�
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound)
                    {
                        //Sk�adnik przepisu nie zosta� znaleziony na talerzu
                        plateContentsMatchesRecipe = false;
                    }
                }
                if(plateContentsMatchesRecipe) 
                {
                    //W�a�ciwe zam�wienie
                    Debug.Log("W�a�ciwe zam�wienie");
                    waitingRecipeSOList.RemoveAt(i);
                    return;
                }   
            }
        }
        //Brak zam�wienia
        //Gracz dostarczy� z�e zam�wienie 
        Debug.Log("Niew�a�ciwe zam�wienie");
    }

}
