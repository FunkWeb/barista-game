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
        private List<DayData> _days;
        
        private int _currentDayIndex = 0;
        public int CurrentDayIndex
        {
            get {return _currentDayIndex;}
            set {_currentDayIndex = Mathf.Clamp(_currentDayIndex, 0, _days.Count-1);}
        }

        public DayData CurrentDayData
        {
            get { return _days[CurrentDayIndex];}
        }
        

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
            _shiftTimer = _days[CurrentDayIndex].ShiftTime;
            _timer = _days[CurrentDayIndex].InitialCustomerDelay;
            
        }


        private void Update()
        {
            _shiftTimer -= Time.deltaTime;
            if (_shiftTimer <= 0f)
            {
                Valley.MonoBehaviourSingleton<SceneLoader>.Instance.LoadPostShiftScene();
                Debug.Log("Shift over. You made it.");
            }
            
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                //Spawn new Customer, choose random type from current days possible customers list.
                var thisEvent = new SpawnCustomer{
                    customerType = _days[CurrentDayIndex].PossibleCustomers.HashSet.ElementAt(Random.Range(0, _days[CurrentDayIndex].PossibleCustomers.HashSet.Count))};
                
                EventBus<SpawnCustomer>.Raise(thisEvent);

                //Set new countdown timer for next spawn.
                _timer = Random.Range(_days[CurrentDayIndex].MinTimeBetweenCustomers, _days[CurrentDayIndex].MaxTimeBetweenCustomers);
            }
        }
    }
}
