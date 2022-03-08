using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class DatabaseTester : MonoBehaviour
    {
        private DataSets _database;

        void Start()
        {
            _database = GetComponent<DataSets>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach(DrinkRecipeData dr in _database.DrinkRecipes)
                {
                    Debug.Log(dr.Name);
                }
                foreach(MainIngredientData mi in _database.MainIngredients)
                {
                    Debug.Log(mi.Name);
                }
                foreach(SideIngredientData si in _database.SideIngredients)
                {
                    Debug.Log(si.Name);
                    if (_database.SideIngredients.Contains(si))
                        Debug.Log("YES!");
                }
            }
        }
    }
}
