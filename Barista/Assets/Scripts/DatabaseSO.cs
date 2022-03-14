using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley;

namespace Funksoft.Barista
{
    [CreateAssetMenu(menuName = "DataObject/Database", fileName = "new database SO")]
    public class DatabaseSO : ScriptableObject
    {
        [SerializeField]
        public SerializableHashSet<DrinkRecipeData> DrinkRecipes;

        [SerializeField]
        public SerializableHashSet<MainIngredientData> MainIngredients;

        [SerializeField]
        public SerializableHashSet<SideIngredientData> SideIngredients;
    }
}
