using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;

namespace Funksoft.Barista
{
    public class MainDispenser : MonoBehaviour
    {
        [SerializeField]
        public MainIngredientData Ingredient;

        [SerializeField]
        public float FillAmountPerSec = 50f;

        public struct Used : IEvent
        {
            public MainIngredientData ingredient;
            public float amount;
        }

        public void Use()
        {
            var useEvent = new Used{ingredient = this.Ingredient, amount = this.FillAmountPerSec * Time.deltaTime};
            EventBus<Used>.Raise(useEvent);
        }



    }
}
