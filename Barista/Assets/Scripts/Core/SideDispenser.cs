using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;

namespace Funksoft.Barista
{
    public class SideDispenser : MonoBehaviour, IClickable
    {
        [SerializeField]
        public SideIngredientData Ingredient;

        public struct Used : IEvent
        {
            public SideIngredientData ingredient;
        }

        public void Use()
        {
            EventBus<Used>.Raise(new Used{ingredient = this.Ingredient});
        }

        //Activated by ClickableObject component when clicked. Completes IClickable interface
        public void OnActivation()
        {
            Use();
        }

    }
}
