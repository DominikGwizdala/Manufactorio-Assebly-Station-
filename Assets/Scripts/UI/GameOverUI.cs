using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private TextMeshProUGUI scoreGainedText;

    private const string CURRENT_HIGHSCORE = "Highscore";

    private float highscore = 0;

    private void Awake()
    {
        highscore = PlayerPrefs.GetFloat(CURRENT_HIGHSCORE, 0f);
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            Show();
            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
            scoreGainedText.text = DeliveryManager.Instance.GetScoreGained().ToString();
            if (DeliveryManager.Instance.GetScoreGained() > highscore)
            {
                scoreGainedText.text = DeliveryManager.Instance.GetScoreGained().ToString() + "\nNEW HIGHSCORE";
                PlayerPrefs.SetFloat(CURRENT_HIGHSCORE, DeliveryManager.Instance.GetScoreGained());
                PlayerPrefs.Save();
            }
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
