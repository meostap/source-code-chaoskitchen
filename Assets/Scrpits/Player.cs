    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    public class Player : MonoBehaviour, IKitchenObjectParent
    {
        public static Player Instace {  get; private set; }

        public event EventHandler OnPickedSomething;
        [SerializeField] private JoystickController joystick;

        public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
        public class OnSelectedCounterChangedEventArgs : EventArgs
        {
            public BaseCounter selectedCouter;
        }


        [SerializeField] public float moveSpeed;
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
            if (!KitchenGameManager.Instance.IsGamePlaying()) return;
            if (selectedCouter != null)
            {
                selectedCouter.InteractAlternate(this);
            }
        }

   

        private void GameInput_OnInteracAction(object sender, System.EventArgs e)
        {
            if (!KitchenGameManager.Instance.IsGamePlaying()) return;
            if (selectedCouter != null)
            {
                selectedCouter.Interact(this);
            }
        }
    public void PickupButton()
    {
        if (KitchenGameManager.Instance.IsGamePlaying() && selectedCouter != null)
        {
            selectedCouter.Interact(this);
        }
    }
    public void ChoppingButton()
    {
        if (KitchenGameManager.Instance.IsGamePlaying() && selectedCouter != null)
        {
            if (selectedCouter is CuttingCounter cuttingCounter)
            {
                // Kiểm tra nếu CuttingCounter có vật phẩm
                if (cuttingCounter.HasKitchenObject())
                {
                    // Thực hiện InteractAlternate nếu có vật phẩm
                    selectedCouter.InteractAlternate(this);
                }
            }
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
        Vector2 inputVectorKeyboard = gameInput.GetMovementVectorNormalized();
        Vector2 inputVectorJoystick = joystick.GetInputVector();
        Vector2 combinedInputVector = inputVectorJoystick.magnitude > 0 ? inputVectorJoystick : inputVectorKeyboard;

        // Sử dụng hướng di chuyển cuối cùng để thực hiện raycast
        Vector3 moveDir = new Vector3(combinedInputVector.x, 0f, combinedInputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir; // Lưu lại hướng tương tác cuối cùng
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, couterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCouter)
                {
                    SetSelectedCouter(baseCounter); // Gán Counter đã chọn
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
        Vector2 inputVectorKeyboard = gameInput.GetMovementVectorNormalized();
        // Lấy input từ joystick
        Vector2 inputVectorJoystick = joystick.GetInputVector();


        // Kết hợp cả hai giá trị input
        Vector2 combinedInputVector = inputVectorKeyboard.magnitude > 0 ? inputVectorKeyboard : inputVectorJoystick;

        Vector3 moveDir = new Vector3(combinedInputVector.x, 0f, combinedInputVector.y);

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
