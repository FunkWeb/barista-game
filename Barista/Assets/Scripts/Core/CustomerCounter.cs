using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using pEventBus;

namespace Funksoft.Barista
{
    public class CustomerCounter : MonoBehaviour, IEventReceiver<DayManager.SpawnCustomer>, IEventReceiver<Customer.Leave>
    {
        [SerializeField]
        private bool _debugLogsEnabled = false;

        [SerializeField]
        private DatabaseSO _database;

        [SerializeField]
        private GameObject _customerPrefab;

        [SerializeField, Header("Properties")]
        private int _maxCustomerCount = 5;

        [SerializeField]
        private float _distanceBetweenCustomers = 3f;

        public List<Customer> Customers = new List<Customer>();

        private void OnEnable()
        {
            EventBus.Register(this);
        }
        private void OnDisable()
        {
            EventBus.UnRegister(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CreateNewCustomer(_database.CustomerTypes.HashSet.ElementAt(Random.Range(0, _database.CustomerTypes.HashSet.Count)));
            }
        }

        #region Receive pEventBus events
        //Receive order to spawn new customer from Day/shift manager
        public void OnEvent(DayManager.SpawnCustomer e)
        {
            CreateNewCustomer(e.customerType);
        }
        //Receive event from spawned customer when they leave
        public void OnEvent(Customer.Leave e)
        {
            CustomerLeft(e.customer, e.statisfied);
        }
        #endregion

        //Remove customers from queue and handle changes needed when customers 
        private void CustomerLeft(Customer customer, bool wasStatisfied)
        {    
            if (Customers.Contains(customer))
            {
                //Remove customer from counter list
                Customers.Remove(customer);
                Destroy(customer.gameObject);
                
                //Handle stats and results based of failed or completed customer
                if (wasStatisfied)
                {
                    PersistentShiftStats.Instance.CompletedCustomers += 1;
                    if (_debugLogsEnabled)
                        TestUI.Log("Customer " + customer.CustomerData.name + " left statisfied with their order.");
                    return;
                }
                if (_debugLogsEnabled)
                    TestUI.Log("Customer " + customer.CustomerData.name + " stomped off unhappy without their order.");
                PersistentShiftStats.Instance.FailedCustomers += 1;
                
            }  
        }

        private void CreateNewCustomer(CustomerData customerData)
        {
            if (Customers.Count >= _maxCustomerCount)
                return;

            //TODO: Instantiate customer UI object.
            
            //Spawn customer prefab instance in position corresponding to the next open spot in the queue.
            Vector3 spawnPos = new Vector3(transform.position.x + (_distanceBetweenCustomers * Customers.Count), transform.position.y, transform.position.z);
            var inst = Instantiate(_customerPrefab, spawnPos, Quaternion.identity);
            //
            Customer customer;
            inst.TryGetComponent<Customer>(out customer);
            Customers.Add(customer);
            customer.CustomerData = customerData;
        }
    }
}
