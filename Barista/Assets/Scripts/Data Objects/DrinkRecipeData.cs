using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    [CreateAssetMenu(menuName = "DataObject/DrinkRecipe", fileName = "DR_RecipeName")]
    public class DrinkRecipeData : ScriptableObject
    {
        [SerializeField]
        public string Name;
        [SerializeField]
        public List<MainIngredientData> Ingredients;
    }
}
