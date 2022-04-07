using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;
using System.Linq;

namespace Funksoft.Barista
{
    public class DayManager : MonoBehaviour
    {
        [SerializeField]
        private DatabaseSO _databaseSO;

        [SerializeField]
        private CustomerQueue _customerQueue;

        private int _currentDayIndex = 0;
        private List<DayData> _days;

        private float _shiftTimer;
        private float _timer;

        //Todo: replace with better eventbus notification of timers triggering and such
        public struct SpawnCustomer : IEvent
        {
            public CustomerData customerType;
        }

        private void Start()
        {
            _days = new List<DayData>(_databaseSO.Days.HashSet);
            _shiftTimer = _days[_currentDayIndex].ShiftTime;
            _timer = _days[_currentDayIndex].InitialCustomerDelay;
            
        }


        private void Update()
        {
            _shiftTimer -= Time.deltaTime;
            if (_shiftTimer <= 0f)
            {
                TestUI.Log("Shift over. You made it.");
                Debug.Log("Shift over. You made it.");
            }
            
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                //Spawn new Customer
                var thisEvent = new SpawnCustomer{customerType = _databaseSO.CustomerTypes.HashSet.ElementAt(Random.Range(0, _databaseSO.CustomerTypes.HashSet.Count))};
                EventBus<SpawnCustomer>.Raise(thisEvent);

                //Set new countdown timer for next spawn.
                _timer = Random.Range(_days[_currentDayIndex].MinTimeBetweenCustomers, _days[_currentDayIndex].MaxTimeBetweenCustomers);
            }
        }
    }
}
