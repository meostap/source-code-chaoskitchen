using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCController : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Waypoints  waypoints;
    [SerializeField] private float speed = 5f;
    private List<Transform> destinationPoints = new List<Transform>();
    public static NPCController Instance { get; private set; }
    public NPChatUI chatUI;
    private Rigidbody rb;
    private Transform currentWaypoint;
    private float rotationSpeed = 200f; 
    private int currentDestinationIndex = 0;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    public event EventHandler OnPickedSomething;
    public event EventHandler<OnNPCSelectedCounterChangedEventArgs> OnNPCSelectedCounterChanged;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private bool hasReachedFinalWaypoint = false;
    

    public class OnNPCSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCouter;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (Instance != null)
        {
            Destroy(gameObject); // Đảm bảo chỉ có 1 instance
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        currentWaypoint = waypoints.GetNextWaypoint(null);
        transform.position = currentWaypoint.position;
        transform.LookAt(currentWaypoint.position);
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
    }



    private void Update()
    {

        if (KitchenGameManager.Instance.IsGamePlaying())
        {  
                MoveToNextDestination();
                if ( currentWaypoint != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.1f)
                    {
                        PerformActionAtDestination();
                        transform.LookAt(currentWaypoint.position);
                        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);

                        if (currentWaypoint == null)
                        {
                            ShowChatAndDisappear();
                            return;
                        }
                    }
                }
        }
    }
    void ShowChatAndDisappear()
    {
        hasReachedFinalWaypoint = true;
        chatUI.gameObject.SetActive(true); // Hiển thị giao diện chat
        chatUI.DisplayBurgerRecipe();
        StartCoroutine(DisplayChatAndDisappear());
    }
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
   

    private void MoveToNextDestination()
    {
        if (currentWaypoint == null) return;

        Vector3 direction = (currentWaypoint.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);

        // Xoay NPC về hướng Waypoint tiếp theo
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    private void PerformActionAtDestination()
    {
        RaycastHit hit;
        Vector3 direction = transform.forward;
        float raycastDistance = 3f;
        //Vector3 direction = destinationPoints[currentDestinationIndex].position - transform.position;
        direction.y = 0;
        if (Physics.Raycast(transform.position, direction.normalized, out hit, raycastDistance))
        {
            if (hit.collider.TryGetComponent(out BaseCounter baseCounter))
            {
                SetSelectedCouter(baseCounter);

                if (!HasKitchenObject()) // Nếu NPC không cầm vật phẩm
                {
                    if (baseCounter is ContainerCounter containerCounter)
                    {
                        containerCounter.Interact(this); // Lấy vật phẩm
                        if (kitchenObject != null)
                        {
                            kitchenObject.SetKitchenObjectParent(this); // Gán NPC làm cha của vật phẩm
                        }
                    }
                    else if (baseCounter is PlatesCounter platesCounter)
                    {
                        platesCounter.Interact(this); // Tương tác với PlatesCounter
                        if (kitchenObject != null)
                        {
                            kitchenObject.SetKitchenObjectParent(this);
                        }
                    }
                    else if (baseCounter is ClearCounter clearCounter)
                    {
                        clearCounter.Interact(this);
                    }
                }
                else
                {
                    baseCounter.Interact(this); // Hành động khác
                }

                StartCoroutine(WaitBeforeNextAction());
            }
        }
    }
    private IEnumerator DisplayChatAndDisappear()
    {
        // Đoạn chat sẽ hiện trong 5 giây
        yield return new WaitForSeconds(2.5f);
        chatUI.gameObject.SetActive(false); // Ẩn UI đoạn chat
        DestroyObject(); // Biến mất NPC
    }

    private IEnumerator WaitBeforeNextAction()
    {
        yield return new WaitForSeconds(1f);
    }

    private void SetSelectedCouter(BaseCounter selectedCouter)
    {
        this.selectedCounter = selectedCouter;
        OnNPCSelectedCounterChanged?.Invoke(this, new OnNPCSelectedCounterChangedEventArgs
        {
            selectedCouter = selectedCouter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
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

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
    
}
