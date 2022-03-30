using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class ServingStation : MonoBehaviour
    {
        [SerializeField]
        private DrinkAssembler _drinkAssembler;
        [SerializeField]
        private CustomerQueue _customerQueue;

        public bool TryServeDrink(Drink drink, Customer customer)
        {
            var order = customer.Order;
            var assembledDrink = _drinkAssembler.AssembleDrink(drink.DrinkMixture);

            if (assembledDrink == null)
            {
                Debug.Log("Mistake: Drink does not match any existing recipe");
                return false;
            }

            if (assembledDrink != order.Drink)
            {
                Debug.Log("Mistake: Wrong drink served. " + customer.CustomerData.name + " ordered: " + order.Drink.Name + ". You served: " + assembledDrink.Name + ".");
                return false;
            }
                
            //Check if drink has all side ingredients ordered.
            foreach(SideIngredientData si in order.SideIngredients)
            {
                if (!drink.DrinkMixture.SideIngredients.HashSet.Contains(si))
                {
                    Debug.Log("Mistake: Drink is missing " + si.Name);
                    return false;
                }
            }
            //Check that drink contains no side ingredients that were not ordered.
            foreach(SideIngredientData si in drink.DrinkMixture.SideIngredients.HashSet)
            {
                if (!order.SideIngredients.Contains(si))
                {
                    Debug.Log("Mistake: Drink was not supposed to have " + si.Name);
                    return false;
                }
            }

            return true;
        }
    }
}
