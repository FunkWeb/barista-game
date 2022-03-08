using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class DrinkMixture
    {
        public List<MainIngredientData> MainIngredients
        {
            get;
            private set;
        }

        public HashSet<SideIngredientData> SideIngredients
        {
            get;
            private set;
        }
    }
}
