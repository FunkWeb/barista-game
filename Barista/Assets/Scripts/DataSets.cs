using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Funksoft.Barista
{
    public class DataSets : MonoBehaviour
    {
        //The horrible data structure duplication is due to unity's inability to serialize HashSets.
        //I am unwilling to deal with the issues of writing a Serializable Hashset myself, 
        //instead I am converting lists.

        public HashSet<DrinkRecipeData> DrinkRecipes
        {
            get 
            {
                //Convert list and return as hashset.
                _drinkRecipeList = _drinkRecipeList.Distinct().ToList();
                return new HashSet<DrinkRecipeData>(_drinkRecipeList);
            }
            private set
            {
                //Remove duplicates when setting new list.
                _drinkRecipeList = value.Distinct().ToList();
            }
        }

        public HashSet<MainIngredientData> MainIngredients
        {
            get 
            {
                _mainIngredientList = _mainIngredientList.Distinct().ToList();
                return new HashSet<MainIngredientData>(_mainIngredientList);
            }
            private set
            {
                _mainIngredientList = value.Distinct().ToList();
            }
        }

        public HashSet<SideIngredientData> SideIngredients
        {
            get 
            {
                _sideIngredientList = _sideIngredientList.Distinct().ToList();
                return new HashSet<SideIngredientData>(_sideIngredientList);
            }
            private set
            {
                _sideIngredientList = value.Distinct().ToList();
            }
        }

        [SerializeField]
        private List<DrinkRecipeData> _drinkRecipeList;
        [SerializeField]
        private List<MainIngredientData> _mainIngredientList;
        [SerializeField]
        private List<SideIngredientData> _sideIngredientList;

        private void Awake()
        {
            
            DrinkRecipes = new HashSet<DrinkRecipeData>(_drinkRecipeList);
            MainIngredients = new HashSet<MainIngredientData>(_mainIngredientList);
            SideIngredients = new HashSet<SideIngredientData>(_sideIngredientList);
        }
    }
}
