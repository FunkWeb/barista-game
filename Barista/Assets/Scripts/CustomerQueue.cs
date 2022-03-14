using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private float _distanceBetweenCustomers = 1.5f;

        private List<Customer> _customers = new List<Customer>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CreateNewCustomer();
            }
        }

        private void CreateNewCustomer()
        {
            if (_customers.Count >= _maxCustomerCount)
                return;
                
            //Spawn customer prefab instance in position corresponding to the next open spot in the queue.
            Vector3 spawnPos = new Vector3(transform.position.x + (_distanceBetweenCustomers * _customers.Count), transform.position.y, transform.position.z);
            var inst = Instantiate(_customerPrefab, spawnPos, Quaternion.identity);
            //Todo: move customer prefab inst to correct position in scene.
            Customer customer;
            inst.TryGetComponent<Customer>(out customer);
            _customers.Add(customer);
        }
    }
}
