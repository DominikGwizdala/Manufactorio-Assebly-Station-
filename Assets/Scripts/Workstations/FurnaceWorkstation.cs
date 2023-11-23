using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static LogCutterWorkstation;

public class FurnaceWorkstation : BaseWorkstation, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Smelting,
        Smelted,
        Oversmelted,
    }

    [SerializeField] private SmeltingRecipeSO[] smeltingRecipeSOArray;
    [SerializeField] private OversmeltingRecipeSO[] oversmeltingRecipeSOArray;

    private State state;
    private float smeltingTimer;
    private float oversmeltingTimer;
    private SmeltingRecipeSO smeltingRecipeSO;
    private OversmeltingRecipeSO oversmeltingRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasWorkshopObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Smelting:
                    smeltingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = smeltingTimer / smeltingRecipeSO.smeltingTimerMax
                    });

                    if (smeltingTimer > smeltingRecipeSO.smeltingTimerMax)
                    {
                        GetWorkshopObject().DestroySelf();
                        WorkshopObject.SpawnWorkshopObject(smeltingRecipeSO.output, this);
                        state = State.Smelted;
                        oversmeltingTimer = 0f;
                        oversmeltingRecipeSO = GetOversmeltingRecipeSOWithInput(GetWorkshopObject().GetWorkshopObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });
                    }
                    break;
                case State.Smelted:
                    oversmeltingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = oversmeltingTimer / oversmeltingRecipeSO.oversmeltingTimerMax
                    });

                    if (oversmeltingTimer > oversmeltingRecipeSO.oversmeltingTimerMax)
                    {
                        GetWorkshopObject().DestroySelf();
                        WorkshopObject.SpawnWorkshopObject(oversmeltingRecipeSO.output, this);
                        state = State.Oversmelted;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Oversmelted:
                    break;
            }
        }
    }
    public override void Interact(Player player)
    {
        if (!HasWorkshopObject())
        {
            if (player.HasWorkshopObject())
            {
                if (HasRecipeWithInput(player.GetWorkshopObject().GetWorkshopObjectSO()))
                {
                    player.GetWorkshopObject().SetWorkshopObjectParent(this);

                    smeltingRecipeSO = GetSmeltingRecipeSOWithInput(GetWorkshopObject().GetWorkshopObjectSO());
                    state = State.Smelting;
                    smeltingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = smeltingTimer / smeltingRecipeSO.smeltingTimerMax
                    });
                }
            }
        }
        else
        {
            if (!player.HasWorkshopObject())
            {
                GetWorkshopObject().transform.rotation = player.transform.rotation;
                GetWorkshopObject().SetWorkshopObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
            else
            {
                if (player.GetWorkshopObject().TryGetPackage(out PackageWorkshopObject packageWorkshopObject))
                {
                    if (packageWorkshopObject.TryAddPart(GetWorkshopObject().GetWorkshopObjectSO()))
                    {
                        GetWorkshopObject().DestroySelf();

                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
        }
    }
    private bool HasRecipeWithInput(WorkshopObjectSO inputWorkshopObjectSO)
    {
        SmeltingRecipeSO smeltingRecipeSO = GetSmeltingRecipeSOWithInput(inputWorkshopObjectSO);
        return smeltingRecipeSO != null;
    }
    private WorkshopObjectSO GetOutputForInput(WorkshopObjectSO inputWorkshopObjectSO)
    {
        SmeltingRecipeSO smeltingRecipeSO = GetSmeltingRecipeSOWithInput(inputWorkshopObjectSO);
        if (smeltingRecipeSO != null)
        {
            return smeltingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }
    private SmeltingRecipeSO GetSmeltingRecipeSOWithInput(WorkshopObjectSO inputWorkshopObjectSO)
    {
        foreach (SmeltingRecipeSO smeltingRecipeSO in smeltingRecipeSOArray)
        {
            if (smeltingRecipeSO.input == inputWorkshopObjectSO)
            {
                return smeltingRecipeSO;
            }
        }
        return null;
    }
    private OversmeltingRecipeSO GetOversmeltingRecipeSOWithInput(WorkshopObjectSO inputWorkshopObjectSO)
    {
        foreach (OversmeltingRecipeSO oversmeltingRecipeSO in oversmeltingRecipeSOArray)
        {
            if (oversmeltingRecipeSO.input == inputWorkshopObjectSO)
            {
                return oversmeltingRecipeSO;
            }
        }
        return null;
    }

    public bool IsSmelted()
    {
        return state == State.Smelted;
    }
}
