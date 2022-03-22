using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class Dispenser : MonoBehaviour
    {
        [SerializeField]
        private MainIngredientData Ingredient;

        [Header("_Temporary_ ImGUI stuff"), SerializeField]
        private Drink _drink;

        [SerializeField]
        private float _buttonWidth = 100f;
        [SerializeField]
        private float _buttonHeight = 100f;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }
        
        //Eventually replace with proper in-worldspace object detected by raycast
        private void OnGUI()
        {
            //Buttons screen position determined by this objects in world position
            Vector3 pos = _mainCamera.WorldToScreenPoint(transform.position);

            //Set properties and values of button and its text
            var style = new GUIStyle(GUI.skin.button);
            style.fontSize = 30;
            
            if (GUI.Button(new Rect(pos.x, Screen.height - pos.y, _buttonWidth, _buttonHeight), Ingredient.Name, style))
            {
                _drink.AddMainIngredient(Ingredient, 0.1f);
            }
        }
    }
}
