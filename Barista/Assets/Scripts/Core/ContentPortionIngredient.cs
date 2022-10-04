using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Funksoft.Barista
{
    public class ContentPortionIngredient : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _ingredientNameText;
        [SerializeField]
        private TextMeshProUGUI _percentageText;
        [SerializeField]
        private Image _fillImage;

        public string IngredientText
        {
            set
            {
                _ingredientNameText.text = value;
            }
        }

        public float Percentage
        {
            //Format the value set and display it as the percentage text.
            set { _percentageText.text = Mathf.RoundToInt(Mathf.Clamp(value, 0, 100)) + "%"; }
        }

        public Color FillColor
        {
            set { _fillImage.color = value;}
        }
    }
}
