using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class TestUI : MonoBehaviour
    {
        [SerializeField, Header("Variables and Properties")]
        private float _buttonWidth = 200f; //Width of standard size button, in screen space units (pixels).
        [SerializeField]
        private float _buttonHeight = 75f; //Height of standard size button, in screen space units (pixels).
        [SerializeField]
        private float _xPadding = 50f; //Outermost padding on horizontal sides, in screen space units. (pixels).
        [SerializeField]
        private float _yPadding = 50f; //Outermost padding on vertical sides, in screen space units. (pixels).
        [SerializeField]
        private float _spaceBetweenButtons = 50f; //Spac between buttons in the same row, in screen space units. (pixels).


        #region References
        [SerializeField, Header("References")]
        private Drink _drink;

        [SerializeField]
        private DrinkAssembler _drinkAssembler;

        [SerializeField]
        private List<Dispenser> _mainDispensers;

        [SerializeField]
        private List<Dispenser> _sideDispensers;

        private Camera _mainCamera;

        #endregion
        
        private Vector3 _topLeft;
        private Vector3 _topRight;
        private Vector3 _botLeft;
        private Vector3 _botRight;


        private void Awake()
        {
            _mainCamera = Camera.main;

            _topLeft = new Vector3(0f, 0f, 0f);
            _topRight = new Vector3(Screen.width, 0f);
            _botLeft = new Vector3(0f, Screen.height, 0f);
            _botRight = new Vector3(Screen.width,Screen.height, 0f);
        }


        private void OnGUI()
        {
            //Set properties and values for buttons and text.
            var style = new GUIStyle(GUI.skin.button);
            style.fontSize = 30;
            
            //MainIngredient dispenser buttons
            for(var i = 0; i < _mainDispensers.Count; i++)
            {
                Dispenser dispenser = _mainDispensers[i];

                //Set position and size of buttons.
                var rect = new Rect(_topLeft.x + _xPadding + i * (_buttonWidth + _spaceBetweenButtons), 
                                    _topLeft.y + _yPadding,
                                    _buttonWidth, 
                                    _buttonHeight);

                //Create button and detect button input.
                if (GUI.RepeatButton(rect, dispenser.Ingredient.Name, style))
                    _drink.AddMainIngredient(dispenser.Ingredient, dispenser.FillAmountPerSec * Time.deltaTime);

            }

            #region Clear Drink Button
            //Set position and size of button
            var clearButtonRect = new Rect(_botLeft.x + _xPadding, _botLeft.y - _yPadding - _buttonHeight , _buttonWidth, _buttonHeight);
            
            //Create button and detect button input
            if (GUI.Button(clearButtonRect, "Clear Drink", style))
                _drink.Clear();
            #endregion

            #region Assemble Drink Button
            //Set position and size of button
            var assembleButtonRect = new Rect(_botRight.x - _xPadding - _buttonWidth, _botRight.y - _yPadding - _buttonHeight, _buttonWidth, _buttonHeight);
            
            //Create button and detect button input
            if (GUI.Button(assembleButtonRect, "Assemble", style))
                _drinkAssembler.AssembleDrink(_drink.DrinkMixture);
            #endregion
            
        }
    }
}
