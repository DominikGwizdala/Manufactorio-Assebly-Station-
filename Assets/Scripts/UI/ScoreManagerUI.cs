using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManagerUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI scoreValueText;

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;


    }

    private void DeliveryManager_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        scoreValueText.text = DeliveryManager.Instance.GetScoreGained().ToString();
    }
}
