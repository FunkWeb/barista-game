using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Funksoft.Barista
{
    public class OrderDisplayPanel : MonoBehaviour
    {
        [SerializeField]
        private Order _order;

        [SerializeField]
        private TextMeshProUGUI _sideIngText;

        
        private void DisplayOrderData()
        {
            foreach(SideIngredientData si in _order.SideIngredients)
            {
                _sideIngText.text += "test";
            }
        }
    }
}
