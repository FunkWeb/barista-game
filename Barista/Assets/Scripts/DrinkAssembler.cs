using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Funksoft.Barista
{
    public class DrinkAssembler : MonoBehaviour
    {
        [SerializeField]
        private DatabaseSO _databaseSO;

        [SerializeField]
        private Drink _drink;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            //Todo: revisit and check if still required after recipe refactor.
            //Consistently order the ingredients in each recipe so we can match each one to the mixture's easier.
            foreach(DrinkRecipeData r in _databaseSO.DrinkRecipes.HashSet)
                r.Ingredients = r.Ingredients.OrderBy(e => e.Name).ToList();
            
        }
        
        private DrinkRecipeData GetMatchingRecipe(DrinkMixture drinkMixture)
        {
            

            //Compare drinkmixture with each recipe to check if any match
            foreach(DrinkRecipeData recipe in _databaseSO.DrinkRecipes.HashSet)
            {
                //Holds the drinkmixture contents after conversion, for matching with recipe.
                List<MainIngredientData> convertedIngredients = new List<MainIngredientData>();
                
                //Convert the amounts of each ingredient in the mixture to match the slot format of the current recipe we are checking against.
                foreach(KeyValuePair<MainIngredientData, float> pair in drinkMixture.MainIngredients)
                {
                    //Convert the amount of liquid for this ingredient to the closest slot, assuming the total amount of liquid corresponds to the total amount of slots
                    int liquidAmountInSlots = Mathf.RoundToInt(pair.Value * recipe.Ingredients.Count);
                    //Add a 
                    for(int i = 0; i < liquidAmountInSlots; i++)
                        convertedIngredients.Add(pair.Key);
                }
                //Sort to match order of Ingredient List in DatabaseSO, so we can compare them without ordering issues
                convertedIngredients = convertedIngredients.OrderBy(e => e.Name).ToList();

                if (Enumerable.SequenceEqual(convertedIngredients, recipe.Ingredients))
                    return recipe;
            }
            //If we reach the end with no match, there is no matching recipe.
            return null;
        }

        //Clear cup contents button. Eventually replace with proper in-worldspace object detected by raycast
        private void OnGUI()
        {
            //Buttons screen position determined by this objects in world position
            Vector3 pos = _mainCamera.WorldToScreenPoint(transform.position);

            //Set properties and values of button and its text
            var style = new GUIStyle(GUI.skin.button);
            style.fontSize = 30;
            
            if (GUI.Button(new Rect(pos.x, Screen.height - pos.y, 400, 100), "Assemble Drink", style))
            {
                Debug.Log(GetMatchingRecipe(_drink.DrinkMixture)?.Name);
            }
        }
    }
}
