using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance { get; private set; }

    [SerializeField] private GameObject firstPage;
    [SerializeField] private GameObject secondPage;
    [SerializeField] private GameObject creditsPage;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button closeButton2;

    private void Awake()
    {
        Instance = this;

        tutorialButton.onClick.AddListener(() => {
            firstPage.SetActive(true);
            secondPage.SetActive(false);
            creditsPage.SetActive(false);
        });

        nextButton.onClick.AddListener(() => {
            firstPage.SetActive(false);
            secondPage.SetActive(true);
        });

        prevButton.onClick.AddListener(() => {
            firstPage.SetActive(true);
            secondPage.SetActive(false);
        });
        creditsButton.onClick.AddListener(() => {
            creditsPage.SetActive(true);
            firstPage.SetActive(false);
            secondPage.SetActive(false);
        });
        closeButton.onClick.AddListener(() => {
            firstPage.SetActive(true);
            secondPage.SetActive(false);
            Hide();
        });
        closeButton2.onClick.AddListener(() => {
            creditsPage.SetActive(false);
            firstPage.SetActive(true);
            secondPage.SetActive(false);
            Hide();
        });
    }

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
