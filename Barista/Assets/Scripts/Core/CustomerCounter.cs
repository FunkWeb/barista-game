using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using pEventBus;
using TMPro;

namespace Funksoft.Barista
{
    public class CustomerCounter : MonoBehaviour, 
                                   IEventReceiver<DayManager.SpawnCustomer>, IEventReceiver<Customer.Leave>, 
                                   IEventReceiver<MoveCamera.CamMoveFinished>, IEventReceiver<MoveCamera.CamMoveStarted>
    {
        [SerializeField]
        private bool _debugLogsEnabled = false;

        [SerializeField]
        private DatabaseSO _database;

        [SerializeField]
        private GameObject _customerPrefab;
        [SerializeField]
        private List<CustomerUI> _customerUIs;

        [SerializeField, Header("Properties")]
        private int _maxCustomerCount = 5;

        [SerializeField]
        private List<Transform> _customerPositions;

        [SerializeField]
        private float _customerUIHeight = 3f;

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

        //Change Scale of CustomerUI object to make it visible, when fully moved from right to left screen
        public void OnEvent(MoveCamera.CamMoveFinished e )
        {
            if (e.screenMovedTo == MoveCamera.Screen.LeftScreen)
            {
                foreach(CustomerUI CUI in _customerUIs)
                {
                    CUI.transform.localScale = new Vector3(1,1,1);
                }
            }
        }
        //Change Scale of CustomerUI object to make it invisible, when starting a move from left to right screen
        public void OnEvent(MoveCamera.CamMoveStarted e)
        {
            if (e.screenMovingTo == MoveCamera.Screen.RightScreen)
            {
                foreach(CustomerUI CUI in _customerUIs)
                {
                    CUI.transform.localScale = new Vector3(0,0,0);
                }
            }
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
            
            //Spawn customer prefab instance in position corresponding to the next open spot in the queue.
            Vector3 spawnPos = _customerPositions[Customers.Count].position;
            var inst = Instantiate(_customerPrefab, spawnPos, Quaternion.identity);
            //
            Customer customer;
            inst.TryGetComponent<Customer>(out customer);
            Customers.Add(customer);
            customer.CustomerData = customerData;

            #region Create CustomerUI
            //Set info, and activate customerUI
            var thisCustomerUI = _customerUIs[Customers.Count-1];
            thisCustomerUI.Customer = customer;
            thisCustomerUI.gameObject.SetActive(true);
            thisCustomerUI.transform.position = Camera.main.WorldToScreenPoint(new Vector3(spawnPos.x, spawnPos.y + _customerUIHeight, spawnPos.z));


            #endregion
        }
    }
}
