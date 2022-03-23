using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class Drink : MonoBehaviour
    {
        [SerializeField]
        public DrinkMixture DrinkMixture = new DrinkMixture();

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void AddSideIngredient(SideIngredientData ingredient)
        {
            //Add if not already added.
            if (DrinkMixture.SideIngredients.HashSet.Add(ingredient))
                Debug.Log("Side ingredient " + ingredient.Name + " added to mixture.");
            else
                Debug.Log("Side ingredient " + ingredient.Name + " is already in mixture.");
        }

        private void Clear()
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
            if (DrinkMixture.GetTotalLiquid + amount < DrinkMixture.MaxCupLiquid)
            {
                DrinkMixture.MainIngredients[ingredient] += amount;
            }
            else //If it would overflow, fill to the brim instead.
            {
                DrinkMixture.MainIngredients[ingredient] += (DrinkMixture.MaxCupLiquid - DrinkMixture.GetTotalLiquid);
            }

            Debug.Log("DrinkMixture contains: " + DrinkMixture.MainIngredients[ingredient] + " of " + ingredient.Name);
            Debug.Log("Total liquid: " + DrinkMixture.GetTotalLiquid);
        }

        //Clear cup contents button. Eventually replace with proper in-worldspace object detected by raycast
        private void OnGUI()
        {
            //Buttons screen position determined by this objects in world position
            Vector3 pos = _mainCamera.WorldToScreenPoint(transform.position);

            //Set properties and values of button and its text
            var style = new GUIStyle(GUI.skin.button);
            style.fontSize = 30;
            
            if (GUI.Button(new Rect(pos.x, Screen.height - pos.y, 400, 100), "Clear Drink Contents", style))
            {
                Clear();
            }
        }

        
    }
}
