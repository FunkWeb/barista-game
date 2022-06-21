using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using pEventBus;

namespace Funksoft.Barista
{
    public class Customer : MonoBehaviour, IClickable
    {
        [SerializeField]
        private bool _debugLogsEnabled = false;

        [SerializeField]
        private DatabaseSO _database;

        [SerializeField]
        public CustomerData CustomerData;

        public Order Order;
        
        public float TimeRemaining;

        public int FailedServingsCount = 0;

        private SpriteRenderer _spriteRenderer;

        public struct Leave : IEvent
        {
            public Customer customer;
            public bool statisfied;
        }

        public struct ServeDrinkInput : IEvent
        {
            public Customer customer;
        }

        private void Awake()
        {
            TryGetComponent<SpriteRenderer>(out _spriteRenderer);
        }

        private void Start()
        {
            _spriteRenderer.sprite = CustomerData.Sprite;
            TimeRemaining = CustomerData.PatienceTimer;
            if (_debugLogsEnabled)
            {
                TestUI.Log("Customer Type: " + CustomerData.name + ". Patience Time: " + CustomerData.PatienceTimer);
            }
            CreateRandomOrder();
        }

        private void Update()
        {
            
            //Customer Patience Countdown. Leave when timer has run out.
            TimeRemaining -= Time.deltaTime;
            if (TimeRemaining < Mathf.Epsilon)
            {
                LeaveUnstatisfied();
            }
        }

        public void OnActivation()
        {
            EventBus<ServeDrinkInput>.Raise(new ServeDrinkInput{ customer = this});
        }
        //Provide sprites for this object's clickable component states.
        public Sprite GetHoverSprite()
        {
            return CustomerData.HoverSprite;
        }
        public Sprite GetClickedSprite()
        {
            return CustomerData.ClickedSprite;
        }

        public void ServedWrongOrder()
        {
            //Play bad groan sound
            if (FailedServingsCount < CustomerData.MaxServeTries)
            {
                //Wrong order, but wont leave yet
                FailedServingsCount++;
                return;
            }
            LeaveUnstatisfied();
        }

        public void LeaveSatisfied()
        {
            EventBus<Leave>.Raise
            (new Leave{ customer = this, statisfied = true });
        }

        private void LeaveUnstatisfied()
        {
            EventBus<Leave>.Raise
            (new Leave{ customer = this, statisfied = false });
        }

        private void CreateRandomOrder()
        {
            //Get random DrinkRecipe from database.
            DrinkRecipeData recipe = _database.DrinkRecipes.HashSet.ElementAt(UnityEngine.Random.Range(0, _database.DrinkRecipes.HashSet.Count));
            HashSet<SideIngredientData> sideIngredients = new HashSet<SideIngredientData>();

            //50-50 chance of each side ingredient getting added to drink.
            foreach(SideIngredientData si in _database.SideIngredients.HashSet)
            {
                if (UnityEngine.Random.Range(0,2) > 0)
                    sideIngredients.Add(si);
            }
            //Create order
            Order = new Order(recipe, sideIngredients, 5f);
                
        }
    }
}
