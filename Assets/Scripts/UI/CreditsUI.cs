using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUi : MonoBehaviour
{
    public static CreditsUi Instance { get; private set; }

    [SerializeField] private Button closeButton;

    private void Awake()
    {
        Instance = this;
        closeButton.onClick.AddListener(() => {
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
