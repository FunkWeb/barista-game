using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    [System.Serializable]
    public class DrinkMixture
    {
        [field: SerializeField]
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

        public int MaxMainIngredients = 3;
    }
}
