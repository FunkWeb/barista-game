using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class Order
    {
        
        public DrinkRecipeData Drink;
        public HashSet<SideIngredientData> SideIngredients;
        public float PatienceTime = 20f;

        public Order(DrinkRecipeData drink, HashSet<SideIngredientData> sideIngredients, float patienceTime)
        {
            Drink = drink;
            SideIngredients = sideIngredients;
            PatienceTime = patienceTime;
        }
    }
}
