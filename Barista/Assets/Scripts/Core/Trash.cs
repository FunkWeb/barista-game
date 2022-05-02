using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class Trash : MonoBehaviour, IClickable
    {
        [SerializeField]
        private Drink _drink;

        private void OnAwake()
        {
            if (_drink == null)
                _drink = FindObjectOfType<Drink>();
        }

        public void OnActivation()
        {
            _drink.Clear();
        }
    }
}
