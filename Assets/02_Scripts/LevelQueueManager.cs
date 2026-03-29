using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class LevelQueueManager : MonoBehaviour
{
    public static LevelQueueManager Instance;

    [Header("Customer Set")]
    public GameObject[] normalCustomerPrefabs; 
    public GameObject[] dreamCustomerPrefabs;  

    [Header("line set")]
    public int maxCustomersInQueue = 3;

    [Header("Customer Rythm")]

    public float spawnInterval = 2.5f; 
    private float spawnTimer = 0f;

    [Header("Position Set")]
    public Transform spawnPoint;
    public Transform counterPoint;
    public List<Transform> queuePositions;

    private List<CustomerBehavior> activeCustomers = new List<CustomerBehavior>();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        if (GameStateManager.Instance == null || 
            GameStateManager.Instance.currentPhase == GameStateManager.GamePhase.Starting ||
            GameStateManager.Instance.currentPhase == GameStateManager.GamePhase.Prep ||
            GameStateManager.Instance.currentPhase == GameStateManager.GamePhase.GameOver)
        {
            return; 
        }


        if (activeCustomers.Count == 0)
        {
            spawnTimer += Time.deltaTime * 4f;
        }
        else
        {
            spawnTimer += Time.deltaTime;
        }

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f; 

            if (activeCustomers.Count < maxCustomersInQueue)
            {
                SpawnNewCustomer();
            }
        }
    }

    void SpawnNewCustomer()
    {
        if (normalCustomerPrefabs == null || normalCustomerPrefabs.Length == 0) return;
        if (dreamCustomerPrefabs == null || dreamCustomerPrefabs.Length == 0) return;
        if (spawnPoint == null) return;

        GameObject prefabToSpawn = null;

        if (GameStateManager.Instance != null && GameStateManager.Instance.currentPhase == GameStateManager.GamePhase.Chaos)
        {
            int randomIndex = Random.Range(0, dreamCustomerPrefabs.Length);
            prefabToSpawn = dreamCustomerPrefabs[randomIndex];
        }
        else
        {
            int randomIndex = Random.Range(0, normalCustomerPrefabs.Length);
            prefabToSpawn = normalCustomerPrefabs[randomIndex];
        }

        if (prefabToSpawn == null) return;

        GameObject newCustomerObject = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
        newCustomerObject.transform.SetParent(this.transform);

        CustomerBehavior customerBehavior = newCustomerObject.GetComponent<CustomerBehavior>();
        if (customerBehavior != null)
        {
            activeCustomers.Add(customerBehavior);
        }

        MoveCustomersInQueue();

        if (activeCustomers.Count == 1)
        {
            UpdateTargetCustomer();
        }
    }

    void MoveCustomersInQueue()
    {
        for (int i = 0; i < activeCustomers.Count; i++)
        {
            Vector3 targetPos;
            
            if (i == 0) targetPos = counterPoint.position;
            else if (i - 1 < queuePositions.Count) targetPos = queuePositions[i - 1].position;
            else targetPos = spawnPoint.position; 

            activeCustomers[i].transform.DOMove(targetPos, 0.5f).SetEase(Ease.OutQuad);
        }
    }

    public void OnCustomerLeave(CustomerBehavior leavingCustomer)
    {
        activeCustomers.Remove(leavingCustomer);
        Destroy(leavingCustomer.gameObject); 
        MoveCustomersInQueue();
        UpdateTargetCustomer();
    }

    void UpdateTargetCustomer()
    {
        if (activeCustomers.Count > 0)
        {
            if (OrderManager.Instance.currentCustomer != activeCustomers[0])
            {
                OrderManager.Instance.SetTargetCustomer(activeCustomers[0]);
            }
        }
        else
        {
            OrderManager.Instance.SetTargetCustomer(null);
        }
    }
}