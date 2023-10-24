using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesWorkstation : BaseWorkstation
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private WorkshopObjectSO plateWorkshopObjectSO;

    private float spawnPlateTimer;
    [SerializeField] private float spawnPlateTimerMax;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 1;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if (GameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasWorkshopObject())
        {
            if(platesSpawnedAmount > 0)
            {
                platesSpawnedAmount--;

                WorkshopObject.SpawnWorkshopObject(plateWorkshopObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
