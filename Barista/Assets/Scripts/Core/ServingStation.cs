using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;

namespace Funksoft.Barista
{
    public class ServingStation : MonoBehaviour, IEventReceiver<TestUI.ServeInputTriggered>
    {
        [SerializeField]
        private bool _debugLogsEnabled = false;

        private DrinkAssembler _drinkAssembler;

        private void Awake()
        {
            TryGetComponent<DrinkAssembler>(out _drinkAssembler);
        }

        private void OnEnable()
        {
            EventBus.Register(this);
        }
        private void OnDisable()
        {
            EventBus.UnRegister(this);
        }

        //Attempt to serve the drink when the input is received
        public void OnEvent(TestUI.ServeInputTriggered e)
        {
            if (TryServeDrink(e.drink, e.customer))
            {
                if (_debugLogsEnabled)
                    Debug.Log("Customer " + e.customer + "'s order was filled and left satisfied.");
                e.customer.OrderSatisfied();
            }
        }

        public bool TryServeDrink(Drink drink, Customer customer)
        {
            if (customer == null)
                return false;
    
            var assembledDrinkRecipe = _drinkAssembler.AssembleDrink(drink.DrinkMixture);

            //If the converted and assembled drink does not match any recipe
            if (assembledDrinkRecipe == null)
            {
                if (_debugLogsEnabled)
                    Debug.Log("Mistake: Drink does not match any existing recipe");
                return false;
            }

            //The converted and asembled drink result is not the recipe the customer ordered
            if (assembledDrinkRecipe != customer.Order.Drink)
            {
                if (_debugLogsEnabled)
                    Debug.Log("Mistake: Wrong drink served. " + customer.CustomerData.name + " ordered: " + 
                              customer.Order.Drink.Name + ". You served: " + assembledDrinkRecipe.Name + ".");
                return false;
            }
                
            //Check if drink has all side ingredients ordered.
            foreach(SideIngredientData si in customer.Order.SideIngredients)
            {
                if (!drink.DrinkMixture.SideIngredients.HashSet.Contains(si))
                {
                    if (_debugLogsEnabled)
                        Debug.Log("Mistake: Drink is missing " + si.Name);
                    return false;
                }
            }
            //Check that drink contains no side ingredients that were not ordered.
            foreach(SideIngredientData si in drink.DrinkMixture.SideIngredients.HashSet)
            {
                if (!customer.Order.SideIngredients.Contains(si))
                {
                    if (_debugLogsEnabled)
                        Debug.Log("Mistake: Drink was not supposed to have " + si.Name);
                    return false;
                }
            }

            return true;
        }
    }
}
