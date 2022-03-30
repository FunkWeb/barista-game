using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Funksoft.Barista
{
    public class Customer : MonoBehaviour
    {
        [SerializeField]
        private bool _debugLogsEnabled = false;

        [SerializeField]
        private DatabaseSO _database;

        [SerializeField]
        public CustomerData CustomerData;

        public Order Order;
        
        public float TimeRemaining;

        public event Action<Customer> CustomerLeaves; //Todo: Add bool second parameter for leaving statisfied or unstatisfied?

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            TryGetComponent<SpriteRenderer>(out _spriteRenderer);
        }

        private void Start()
        {
            TimeRemaining = CustomerData.PatienceTimer;
            _spriteRenderer.sprite = CustomerData.Sprite;
            if (_debugLogsEnabled)
            {
                Debug.Log("Customer Type: " + CustomerData.name);
                Debug.Log("Patience Time: " + CustomerData.PatienceTimer);
            }
            CreateRandomOrder();
        }

        private void Update()
        {
            
            //Customer Patience Countdown. Leave when timer has run out.
            TimeRemaining -= Time.deltaTime;
            if (TimeRemaining < Mathf.Epsilon)
            {
                OutOfPatience();
            }
        }

        private void OutOfPatience()
        {
            if (CustomerLeaves != null)
                CustomerLeaves(this);
                
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
            if (_debugLogsEnabled)
            {
                Debug.Log("Order: " + Order.Drink.Name);
                foreach(SideIngredientData osi in Order.SideIngredients)
                    Debug.Log("Topping wanted: " + osi.Name);
            }
            
                
        }
    }
}
