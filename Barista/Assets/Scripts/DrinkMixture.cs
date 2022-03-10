using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley;

namespace Funksoft.Barista
{
    [System.Serializable]
    public class DrinkMixture
    {
        [SerializeField]
        public int MaxMainIngredients = 3;

        [field: SerializeField, Header("__Ingredient Contents__")]
        public List<MainIngredientData> MainIngredients
        {
            get;
            private set;
        }

        [field: SerializeField]
        public SerializableHashSet<SideIngredientData> SideIngredients
        {
            get;
            private set;
        }
    }
}
