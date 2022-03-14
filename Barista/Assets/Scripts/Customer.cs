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

        private Order _order;
        
        private float _timeRemaining;

        public event Action<Customer> CustomerLeaves;

        private void Start()
        {
            CreateRandomOrder();
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
            Debug.Log(_order.Drink.Name);
            Debug.Log(_order.PatienceTime);
            foreach(SideIngredientData osi in _order.SideIngredients)
            {
                Debug.Log(osi.Name);
            }
                
        }
    }
}
