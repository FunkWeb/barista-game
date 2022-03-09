using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class Customer : MonoBehaviour
    {
        private Order _order;
        
        private float _timeRemaining;

        private void Start()
        {
            _timeRemaining = _order.PatienceTime;
        }

        private void Update()
        {
            //Customer Patience Countdown. Leave when timer has run out.
            _timeRemaining -= Time.deltaTime;
            if (_timeRemaining < Mathf.Epsilon)
            {
                OutOfPatience();
            }
        }

        private void OutOfPatience()
        {
            //Customer leaves establishment.
            //Order Cancelled.
        }
    }
}
