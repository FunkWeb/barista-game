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
        private DatabaseSO _database;

        [SerializeField]
        public CustomerData CustomerData;

        private Order _order;
        
        private float _timeRemaining;

        public event Action<Customer> CustomerLeaves;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            TryGetComponent<SpriteRenderer>(out _spriteRenderer);
        }

        private void Start()
        {
            _timeRemaining = CustomerData.PatienceTimer;
            _spriteRenderer.sprite = CustomerData.Sprite;
            Debug.Log("Customer Type: " + CustomerData.name);
            Debug.Log("Patience Time: " + CustomerData.PatienceTimer);
            CreateRandomOrder();
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
            if (CustomerLeaves != null)
            {
                CustomerLeaves(this);
            }
                
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
            _order = new Order(recipe, sideIngredients, 5f);
            Debug.Log("Order: " + _order.Drink.Name);
            foreach(SideIngredientData osi in _order.SideIngredients)
            {
                Debug.Log("Topping wanted: " + osi.Name);
            }
                
        }
    }
}
