using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instace {  get; private set; }

    public event EventHandler OnPickedSomething;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCouter;
    }


    [SerializeField] public float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask couterLayerMask;
    [SerializeField] private Transform kichenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCouter;
    private KitchenObject kitchenObject;


    private void Awake()
    {
        if (Instace != null)
        {
            Debug.LogError("There is more than 1 player instance");
        }
        Instace = this;
    }
    private void Start()
    {
        gameInput.OnInteracAction += GameInput_OnInteracAction;
        gameInput.OnInteraAlternatecAction += GameInput_OnInteraAlternatecAction;
    }

    private void GameInput_OnInteraAlternatecAction(object sender, EventArgs e)
    {
        if (selectedCouter != null)
        {
            selectedCouter.InteractAlternate(this);
        }
    }

   

    private void GameInput_OnInteracAction(object sender, System.EventArgs e)
    {
        if (selectedCouter != null)
        {
            selectedCouter.Interact(this);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMoment();
        HandleInteractions();
    }
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, couterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCouter)
                {
                    SetSelectedCouter(baseCounter);

                   
                }

            }
            else
            {
                SetSelectedCouter(null);
            }
        }
        else
        {
            SetSelectedCouter(null);
        }

    }

    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleMoment()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        if (isWalking)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }
    }
    private void SetSelectedCouter(BaseCounter selectedCouter)
    {
        this.selectedCouter = selectedCouter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCouter = selectedCouter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kichenObjectHoldPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {

        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this,EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void CleaKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject() { return kitchenObject != null; }
}
