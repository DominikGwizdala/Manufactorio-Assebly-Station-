using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IWorkshopObjectParent
{
    public event EventHandler OnPickedSomething;
    public static Player Instance { get; private set; }  
    public event EventHandler<OnSelectedWorkstationChangedEventArgs> OnSelectedWorkstationChanged;
    public class OnSelectedWorkstationChangedEventArgs : EventArgs {
        public BaseWorkstation selectedWorkstation;
    }

    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed=10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask workstationsLayerMask;
    [SerializeField] private Transform workshopObjectHoldPoint;

    private bool isRunning;
    private bool isHolding;
    private Vector3 lastInteractDirection;
    private BaseWorkstation selectedWorkstation;
    private WorkshopObject workshopObject;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Jest wiêcej ni¿ jedna instancja Gracza/Playera");
        }
        Instance = this;
    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (selectedWorkstation != null)
        {
            selectedWorkstation.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if (selectedWorkstation != null)
        {
            selectedWorkstation.Interact(this);
        }       
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();     
    }
    private void HandleInteractions() 
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDirection != Vector3.zero )
        {
            lastInteractDirection = moveDirection;
        }
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position,lastInteractDirection,out RaycastHit raycastHit, interactDistance, workstationsLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseWorkstation baseWorkstation))
            {
                //clearWorkstation.Interact();
                if (baseWorkstation != selectedWorkstation) 
                {
                    SetSelectedWorkstation(baseWorkstation); 
                }
            }
            else
            {
                SetSelectedWorkstation(null);
            }
        }
        else
        {
            SetSelectedWorkstation(null);
        }
        //Debug.Log(selectedWorkstation);
    }
    private void HandleMovement() 
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = speed * Time.deltaTime;
        float playerRadius = .6f;
        float playerHeight = .2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0);
            canMove = (moveDirection.x < -.5f || moveDirection.x > +.5f)&& !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);

            if (canMove)
            {
                //Poruszanie sie tylko po osi X
                moveDirection = moveDirectionX;
            }
            else
            {
                //normalized jesli chcemy aby spowolniony po skosie w sciane
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z);
                canMove = (moveDirection.z < -.5f || moveDirection.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);

                if (canMove)
                {
                    //Poruszanie sie tylko po osi Z
                    moveDirection = moveDirectionZ;
                }
            }

        }
        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }

        isRunning = moveDirection != Vector3.zero;
        isHolding = HasWorkshopObject();
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }
    public bool IsRunning()
    {
        return isRunning;
    }
    public bool IsHolding()
    {
        return isHolding;
    }
    private void SetSelectedWorkstation(BaseWorkstation selectedWorkstation)
    {
        this.selectedWorkstation = selectedWorkstation;

        OnSelectedWorkstationChanged?.Invoke(this, new OnSelectedWorkstationChangedEventArgs
        {
            selectedWorkstation = this.selectedWorkstation
        });

    }

    public Transform GetWorkshopObjectFollowTransform()
    {
        return workshopObjectHoldPoint;
    }
    public void SetWorkshopObject(WorkshopObject workshopObject)
    {
        this.workshopObject = workshopObject;
        if(workshopObject != null)
        {
            OnPickedSomething?.Invoke(this,EventArgs.Empty);
        }
    }
    public WorkshopObject GetWorkshopObject()
    {
        return workshopObject;
    }
    public void ClearWorkshopObject()
    {
        workshopObject = null;
    }
    public bool HasWorkshopObject()
    {
        return workshopObject != null;
    }
}
