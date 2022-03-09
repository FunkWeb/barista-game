using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class Drink : MonoBehaviour
    {
        private DrinkMixture _drinkMixture = new DrinkMixture();

        private void AddMainIngredient(MainIngredientData ingredient)
        {
            //Add if mainingredient list isnt full.
            if (_drinkMixture.MainIngredients.Count < _drinkMixture.MaxMainIngredients)
            {
                _drinkMixture.MainIngredients.Add(ingredient);
                Debug.Log("Main ingredient " + ingredient.Name + " added to mixture.");
            }
            else
                Debug.Log("Ingredient " + ingredient.Name + " not added. Mixture is full.");
        }

        private void AddSideIngredient(SideIngredientData ingredient)
        {
            //Add if not already added.
            if (_drinkMixture.SideIngredients.Add(ingredient))
                Debug.Log("Side ingredient " + ingredient.Name + " added to mixture.");
            else
                Debug.Log("Side ingredient " + ingredient.Name + " is already in mixture.");
        }

        private void Clear()
        {
            _drinkMixture = new DrinkMixture();
            Debug.Log("Drink cleared!");
        }
    }
}
