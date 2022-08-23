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

        public bool PreShiftPause = true;
        
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
        

        public float ShiftTimer{get; private set;}
        private float _timer;

        //Todo: replace with better eventbus notification of timers triggering and such
        public struct SpawnCustomer : IEvent
        {
            public CustomerData customerType;
        }

        public struct ShiftEnded : IEvent
        {

        }

        private void Start()
        {

            _days = new List<DayData>(_databaseSO.Days.HashSet);
            ShiftTimer = _days[CurrentDayIndex].ShiftTime;
            _timer = _days[CurrentDayIndex].InitialCustomerDelay;
            
        }
        private void LoadShiftEnd()
        {
            Valley.MonoBehaviourSingleton<SceneLoader>.Instance.LoadPostShiftScene();
            Debug.Log("Shift over. You made it.");
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
                PreShiftPause = !PreShiftPause;
            //Dont start countdown until shift is properly started.
            if (PreShiftPause)
                return;

            //End shift when the time is up
            ShiftTimer -= Time.deltaTime;
            if (ShiftTimer <= 0f)
            {
                //Raise event for end of shift cleanup.
                EventBus<ShiftEnded>.Raise(new ShiftEnded{});
                //Load end of shift scene, Invoke with delay so we are certain all events have a chance to fire, and the player isnt given whiplash.
                Invoke("LoadShiftEnd", 1f);
            }
            
            //Customer spawn timer.
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
