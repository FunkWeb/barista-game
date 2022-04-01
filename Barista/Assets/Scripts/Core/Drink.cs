using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;

namespace Funksoft.Barista
{
    public class Drink : MonoBehaviour, IEventReceiver<MainDispenser.Used>, IEventReceiver<SideDispenser.Used>
    {
        [SerializeField]
        private bool _debugLogsEnabled = false;

        [SerializeField]
        public DrinkMixture DrinkMixture = new DrinkMixture(); //Holds the actual ingredients and amounts of liquid in the drink.

        //Subscribe to events
        private void OnEnable()
        {
            EventBus.Register(this);
        }
        private void OnDisable()
        {
            EventBus.UnRegister(this);
        }

        //Receive events from pEventBus
        public void OnEvent(MainDispenser.Used e)
        {
            AddMainIngredient(e.ingredient, e.amount);
        }
        public void OnEvent(SideDispenser.Used e)
        {
            AddSideIngredient(e.ingredient);
        }

        public void AddMainIngredient(MainIngredientData ingredient, float amount)
        {
            //Add non existant ingredient keys before attempting to change any values. Prevents crash from attempting to change value of yet to be added key.
            if (!DrinkMixture.MainIngredients.ContainsKey(ingredient))
                DrinkMixture.MainIngredients.Add(ingredient, 0f);

            //If cup does not overflow if amount is added, allow it
            if (DrinkMixture.GetTotalLiquid + amount < DrinkMixture.MaxTotalLiquid)
            {
                DrinkMixture.MainIngredients[ingredient] += amount;
            }
            else //If it would overflow, fill to the brim instead.
            {
                DrinkMixture.MainIngredients[ingredient] += (DrinkMixture.MaxTotalLiquid - DrinkMixture.GetTotalLiquid);
            }
        }

        public void AddSideIngredient(SideIngredientData ingredient)
        {
            //Add if not already added.
            if (DrinkMixture.SideIngredients.HashSet.Add(ingredient) && _debugLogsEnabled)
                TestUI.Log("Side ingredient " + ingredient.Name + " added to mixture.");
            else if (_debugLogsEnabled)
                TestUI.Log("Side ingredient " + ingredient.Name + " was not added as it is already in mixture.");
        }

        public void Clear()
        {
            DrinkMixture = new DrinkMixture();
            if (_debugLogsEnabled)
                TestUI.Log("Drink contents emptied.");
        }

        
    }
}

