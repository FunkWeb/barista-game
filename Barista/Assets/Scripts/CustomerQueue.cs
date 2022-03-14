using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Funksoft.Barista
{
    public class CustomerQueue : MonoBehaviour
    {
        [SerializeField]
        private DatabaseSO _database;

        [SerializeField]
        private GameObject _customerPrefab;

        [SerializeField, Header("Queue Properties")]
        private int _maxCustomerCount = 5;

        [SerializeField]
        private float _distanceBetweenCustomers = 3f;

        private List<Customer> _customers = new List<Customer>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CreateNewCustomer();
            }
        }

        private void OnDestroy()
        {
            //Event cleanup, unsubscribe to vents from all customers in queue.
            foreach(Customer c in _customers)
            {
                c.CustomerLeaves -= CustomerLeft;
            }
        }

        //Remove customers from queue and handle changes needed when customers 
        private void CustomerLeft(Customer customer)
        {    
            if (_customers.Contains(customer))
            {
                _customers.Remove(customer);
                Destroy(customer.gameObject);
                //Reorder queue. (Move up sprites to fill gaps left by leaving customers).
                Debug.Log("Customer Left");

            }
                
        }

        private void CreateNewCustomer()
        {
            if (_customers.Count >= _maxCustomerCount)
                return;
                
            //Spawn customer prefab instance in position corresponding to the next open spot in the queue.
            Vector3 spawnPos = new Vector3(transform.position.x + (_distanceBetweenCustomers * _customers.Count), transform.position.y, transform.position.z);
            var inst = Instantiate(_customerPrefab, spawnPos, Quaternion.identity);
            Customer customer;
            inst.TryGetComponent<Customer>(out customer);
            _customers.Add(customer);

            //Subscribe to CustomerLeaves event so we know when to "clean up" and remove them.
            customer.CustomerLeaves += CustomerLeft;
        }
    }
}
