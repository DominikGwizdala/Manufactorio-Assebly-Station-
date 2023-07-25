using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed=10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    private bool isWalking;
    private Vector3 lastInteractDirection;
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        Vector2 inputVector = gameInput.GetMovmentVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDirection != Vector3.zero)
        {
            lastInteractDirection = moveDirection;
        }
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCunter clearCunter))
            {

                clearCunter.Interact();
            }


        }
    }

    private void Update()
    {

        HandleMovment();
       HandleInteractions();

       
    }
    private void HandleInteractions() 
    {
        Vector2 inputVector = gameInput.GetMovmentVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        if(moveDirection != Vector3.zero )
        {
            lastInteractDirection = moveDirection;
        }
        float interactDistance = 2f;
        if(Physics.Raycast(transform.position,lastInteractDirection,out RaycastHit raycastHit, interactDistance,countersLayerMask))
        {
            if(raycastHit.transform.TryGetComponent(out ClearCunter clearCunter)){

                //clearCunter.Interact();
            }
            
                
        }
       
    }
    private void HandleMovment() 
    {
        Vector2 inputVector = gameInput.GetMovmentVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = speed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = .2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);

            if (canMove)
            {
                //Poruszanie sie tylko po osi X
                moveDirection = moveDirectionX;
            }
            else
            {
                //normalized jesli chcemy aby spowolniony po skosie w sciane
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);

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

        isWalking = moveDirection != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }
    public bool IsWalking()
        {
            return isWalking;
        }

}
