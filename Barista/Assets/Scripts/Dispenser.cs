using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class Dispenser : MonoBehaviour
    {
        [SerializeField]
        public MainIngredientData Ingredient;

        [SerializeField]
        public float FillAmountPerSec = 50f;

    }
}
