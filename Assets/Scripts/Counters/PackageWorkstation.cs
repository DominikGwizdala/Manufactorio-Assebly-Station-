using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageWorkstation : BaseWorkstation
{
    public event EventHandler OnPackageSpawned;
    public event EventHandler OnPackageRemoved;

    [SerializeField] private WorkshopObjectSO packageWorkshopObjectSO;

    private float spawnPackageTimer;
    [SerializeField] private float spawnPackageTimerMax;
    private int packageSpawnedAmount;
    private int packageSpawnedAmountMax = 1;

    private void Update()
    {
        spawnPackageTimer += Time.deltaTime;
        if (spawnPackageTimer > spawnPackageTimerMax)
        {
            spawnPackageTimer = 0f;

            if (GameManager.Instance.IsGamePlaying() && packageSpawnedAmount < packageSpawnedAmountMax)
            {
                packageSpawnedAmount++;

                OnPackageSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasWorkshopObject())
        {
            if(packageSpawnedAmount > 0)
            {
                packageSpawnedAmount--;

                WorkshopObject.SpawnWorkshopObject(packageWorkshopObjectSO, player);

                OnPackageRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
