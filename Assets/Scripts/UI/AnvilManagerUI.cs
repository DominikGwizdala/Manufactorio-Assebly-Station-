using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnvilManagerUI : MonoBehaviour
{
    public static AnvilManagerUI Instance { get; private set; }

    [SerializeField] private Button pickaxebutton;
    [SerializeField] private Button axebutton;
    [SerializeField] private Button hoebutton;

    private void Awake()
    {
        Instance = this;

        pickaxebutton.onClick.AddListener(() => {
           
        });
        axebutton.onClick.AddListener(() => {
           
        });
        hoebutton.onClick.AddListener(() => {
            
        });
    }
    private void Start()

    {
        GameManager.Instance.OnAnvilUsed += GameManager_OnAnvilUsed; 
        GameManager.Instance.OnAnvilUnused += GameManager_OnAnvilUnused;

        Hide();
    }
    private void GameManager_OnAnvilUnused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnAnvilUsed(object sender, System.EventArgs e)
    {
        Show();
    }
    public void Show()
    {
        gameObject.SetActive(true);
        pickaxebutton.Select();

    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
