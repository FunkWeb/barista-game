using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    //Class Responsible for on screen testing/debugging GUI and Inputs.
    public class TestUI : MonoBehaviour
    {
        [SerializeField, Header("Panel and Text")]
        private Vector2 _cupPanelPos;
        [SerializeField]
        private Vector2 _drinkPanelPos;
        [SerializeField]
        private Vector2 _customerPanelPos;
        [SerializeField]
        private float _panelWidth = 800f;
        [SerializeField]
        private float _panelHeight = 600f;
        [SerializeField]
        private float _textLabelWidth = 200;
        [SerializeField]
        private float _textLabelHeight = 50;
        [SerializeField]
        private float _textPaddingX = 50;
        [SerializeField]
        private float _textPaddingY = 50;
        [SerializeField]
        private float _spaceBetweenText = 50;

        [SerializeField, Header("Buttons")]
        private float _buttonWidth = 200f; //Width of standard size button, in screen space units (pixels).
        [SerializeField]
        private float _buttonHeight = 75f; //Height of standard size button, in screen space units (pixels).
        [SerializeField]
        private float _buttonPaddingX = 50f; //Outermost padding on horizontal sides, in screen space units. (pixels).
        [SerializeField]
        private float _buttonPaddingY = 50f; //Outermost padding on vertical sides, in screen space units. (pixels).
        [SerializeField]
        private float _spaceBetweenButtons = 50f; //Spac between buttons in the same row, in screen space units. (pixels).


        #region References
        [SerializeField, Header("References")]
        private Drink _drink;

        [SerializeField]
        private DrinkAssembler _drinkAssembler;

        [SerializeField]
        private CustomerQueue _customerQueue;

        [SerializeField]
        private List<Dispenser> _mainDispensers;

        [SerializeField]
        private List<Dispenser> _sideDispensers;

        private Camera _mainCamera;

        #endregion
        
        private DrinkRecipeData _assembledDrink;
        private Customer _currentCustomer;
        private GUIStyle _style;

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
            
            DrawInputButtonsGUI();
            DrawCupPanelGUI(_cupPanelPos);
            if (_assembledDrink)
                DrawDrinkPanelGUI(_assembledDrink, _drinkPanelPos);


            _style.alignment = TextAnchor.MiddleCenter;
            _style.fontSize = 30;

            Rect cTabsRect;
            //Create tab buttons for each customer & store selected customer
            for(var i = 0; i < _customerQueue.Customers.Count; i++)
            {
                cTabsRect = new Rect(_topRight.x - _buttonPaddingX - _buttonWidth/2 - i * (_buttonWidth/2 + _spaceBetweenButtons),
                                     _topRight.y + _buttonPaddingY
                                     , _buttonWidth/2, _buttonHeight);

                if (GUI.Button(cTabsRect, _customerQueue.Customers[i].TimeRemaining.ToString("F2"), _style))
                {
                    _currentCustomer = _customerQueue.Customers[i];
                }
            }
            //if a customer is selected, create customer panel with its data
            if (_currentCustomer)
                DrawCustomerPanel(_customerPanelPos, _currentCustomer);



        }

        private void DrawInputButtonsGUI()
        {
            //Set properties and values for buttons and text.
            _style = new GUIStyle(GUI.skin.button);
            _style.alignment = TextAnchor.MiddleCenter;
            _style.fontSize = 30;
            Rect rect;
            
            //MainIngredient dispenser buttons
            for(var i = 0; i < _mainDispensers.Count; i++)
            {
                Dispenser dispenser = _mainDispensers[i];

                //Set position and size of buttons.
                rect = new Rect(_topLeft.x + _buttonPaddingX + i * (_buttonWidth + _spaceBetweenButtons), 
                                    _topLeft.y + _buttonPaddingY,
                                    _buttonWidth, 
                                    _buttonHeight);

                //Create button and detect button input.
                if (GUI.RepeatButton(rect, dispenser.Ingredient.Name, _style))
                    _drink.AddMainIngredient(dispenser.Ingredient, dispenser.FillAmountPerSec * Time.deltaTime);

            }

            #region Clear Drink Button
            //Set position and size of button
            rect = new Rect(_botLeft.x + _buttonPaddingX, _botLeft.y - _buttonPaddingY - _buttonHeight , _buttonWidth, _buttonHeight);
            
            //Create button and detect button input
            if (GUI.Button(rect, "Clear Drink", _style))
            {
                _assembledDrink = null;
                _drink.Clear();
            }
                
            #endregion

            #region Assemble Drink Button
            //Set position and size of button
            rect = new Rect(_botLeft.x + Screen.width/2 - _buttonWidth/2, _botRight.y - _buttonPaddingY - _buttonHeight, _buttonWidth, _buttonHeight);
            
            //Create button and detect button input. Get resulting drink from DrinkAssembler.
            if (GUI.Button(rect, "Assemble", _style))
                _assembledDrink = _drinkAssembler.AssembleDrink(_drink.DrinkMixture);
                
            #endregion
        }

        private void DrawDrinkPanelGUI(DrinkRecipeData recipe, Vector2 pos)
        {
            //Create Panel and Title text
            var panelRect = new Rect(_topLeft.x + _mainCamera.WorldToScreenPoint(pos).x - _panelWidth/2, 
                                     _topLeft.y + _mainCamera.WorldToScreenPoint(pos).y - _panelHeight/2, 
                                     _panelWidth, _panelHeight);
            _style.alignment = TextAnchor.UpperCenter;
            _style.fontSize = 30;
            GUI.Box(panelRect, "Assembled Drink: " + recipe.Name, _style);

            //Style for text labels
            _style.alignment = TextAnchor.MiddleCenter;
            _style.fontSize = 25;

            Rect textRect;

            for(var i = 0; i < recipe.Ingredients.Count; i++)
            {
                //Set area for text label
                textRect = new Rect(panelRect.x + _textPaddingX, 
                                    panelRect.y + _textPaddingY + i * (_spaceBetweenText + _textLabelHeight), 
                                    _textLabelWidth, _textLabelHeight);
                //Create text label
                GUI.Label(textRect, recipe.Ingredients[i].Name, _style);
            }


        }

        private void DrawCupPanelGUI(Vector2 pos)
        {
            //Create Panel and Title text
            var panelRect = new Rect(_topLeft.x + _mainCamera.WorldToScreenPoint(pos).x - _panelWidth/2, 
                                     _topLeft.y + _mainCamera.WorldToScreenPoint(pos).y - _panelHeight/2, 
                                     _panelWidth, _panelHeight);
            _style.alignment = TextAnchor.UpperCenter;
            GUI.Box(panelRect, "Current Drink Mixture Contents", _style);

            //Set style for text contained in panel
            _style.alignment = TextAnchor.MiddleCenter;
            _style.fontSize = 25;

            int index = 0;
            Rect textRect;
            //Display values for each main ingredient
            foreach(KeyValuePair<MainIngredientData, float> pair in _drink.DrinkMixture.MainIngredients)
            {
                //Set area for text label
                textRect = new Rect(panelRect.x + _textPaddingX, 
                                    panelRect.y + _textPaddingY + index * (_spaceBetweenText + _textLabelHeight), 
                                    _textLabelWidth, _textLabelHeight);
                //Create text label
                GUI.Label(textRect, pair.Key.Name + " : " + pair.Value.ToString("F2") + "ml", _style);
                index++;
            }
            
            //Total liquid text
            textRect = new Rect(panelRect.x, panelRect.y + _panelHeight - _textLabelHeight, _panelWidth, _textLabelHeight);
            GUI.Label(textRect, "Total Liquid: " + _drink.DrinkMixture.GetTotalLiquid.ToString("F2") + "ml / " + _drink.DrinkMixture.MaxTotalLiquid + "ml", _style);
        }

        private void DrawCustomerPanel(Vector2 pos, Customer customer)
        {
            //Create Panel and Title text
            var panelRect = new Rect(_topLeft.x + _mainCamera.WorldToScreenPoint(pos).x - _panelWidth/2, 
                                     _topLeft.y + _mainCamera.WorldToScreenPoint(pos).y - _panelHeight/2, 
                                     _panelWidth, _panelHeight);
            //Set style for panel and title text
            _style.alignment = TextAnchor.UpperCenter;
            _style.fontSize = 30;
            //Create panel and title text
            GUI.Box(panelRect, "Customer: " + customer.CustomerData.name + " | Time Left: " + customer.TimeRemaining.ToString("F2"), _style);

            Rect textRect;
            textRect = new Rect(panelRect.x + _panelWidth / 2 - _textLabelWidth/2, panelRect.y + _textPaddingY, _textLabelWidth, _textLabelHeight);

            _style.alignment = TextAnchor.MiddleCenter;
            GUI.Label(textRect, customer.Order.Drink.Name, _style);
            _style.fontSize = 25;

            //Add text for each main ingredient in customer order
            for(var i = 0; i < customer.Order.Drink.Ingredients.Count; i++)
            {
                //Set area for text label
                textRect = new Rect(panelRect.x + _textPaddingX, 
                                    panelRect.y + _textPaddingY + _textLabelHeight + _spaceBetweenText + i * (_spaceBetweenText + _textLabelHeight), 
                                    _textLabelWidth, _textLabelHeight);
                //Create text label
                GUI.Label(textRect, customer.Order.Drink.Ingredients[i].Name, _style);
            }

            int index = 0;
            foreach(SideIngredientData si in customer.Order.SideIngredients)
            {
                //Set area for text label
                textRect = new Rect(panelRect.x + _panelWidth - _textPaddingX - _textLabelWidth, 
                                    panelRect.y + _textPaddingY + _textLabelHeight + _spaceBetweenText + index * (_spaceBetweenText + _textLabelHeight), 
                                    _textLabelWidth, _textLabelHeight);
                //Create text label
                GUI.Label(textRect, si.Name, _style);
                index++;
            }
        }

        
    }
}
