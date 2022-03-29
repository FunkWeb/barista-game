using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class Drink : MonoBehaviour
    {
        [SerializeField]
        public DrinkMixture DrinkMixture = new DrinkMixture();

        private void AddSideIngredient(SideIngredientData ingredient)
        {
            //Add if not already added.
            if (DrinkMixture.SideIngredients.HashSet.Add(ingredient))
                Debug.Log("Side ingredient " + ingredient.Name + " added to mixture.");
            else
                Debug.Log("Side ingredient " + ingredient.Name + " is already in mixture.");
        }

        public void Clear()
        {
            DrinkMixture = new DrinkMixture();
            Debug.Log("Drink cleared!");
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
    }
}

