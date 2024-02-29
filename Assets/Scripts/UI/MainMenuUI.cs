using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI highscoreText;

    private const string CURRENT_HIGHSCORE = "Highscore";

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });

        tutorialButton.onClick.AddListener(() =>
        {
            TutorialUI.Instance.Show();
        });
       creditsButton.onClick.AddListener(() =>
        {
            TutorialUI.Instance.Show();
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        Time.timeScale = 1f;

        highscoreText.text = "HIGHSCORE: " + PlayerPrefs.GetFloat(CURRENT_HIGHSCORE, 0f); ;
    }
}
