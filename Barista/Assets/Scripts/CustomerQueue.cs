using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class CustomerQueue : MonoBehaviour
    {
        [SerializeField]
        private DataSets _dataSets;

        private List<Customer> _customers = new List<Customer>();

        private void CreateNewCustomer()
        {
            //Create customer
            //Add to _customers list
            //Generate Order
            //Entry animation/movement
        }
    }
}
