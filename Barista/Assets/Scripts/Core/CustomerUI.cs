using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Funksoft.Barista
{
    public class CustomerUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private TextMeshProUGUI _timerText;

        //[SerializeField]
        //private bool _showExtendedInfo = false;

        [SerializeField]
        private GameObject _orderPanel;

        [SerializeField]
        private Image _orderImage;
        [SerializeField]
        private TextMeshProUGUI _orderTitleText;
        [SerializeField]
        private TextMeshProUGUI _orderSideIngredientText;

        private Customer _customer;
        public Customer Customer
        {
            get {return _customer;}
            set
            {
                _customer = value;
                DisplayOrderData();
            }
        }
        public Vector3 WorldPos{ get; set;}


        private void Start()
        {
            _orderPanel.SetActive(false);
            DisplayOrderData();   
        }


        private void LateUpdate()
        {
            _timerText.text = Customer.TimeRemaining.ToString("F0");
        }

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            //_showExtendedInfo = true;
            _orderPanel.SetActive(true);
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            //_showExtendedInfo = false;
            _orderPanel.SetActive(false);
        }

        //Give display components the relevant data
        private void DisplayOrderData()
        {

            _orderTitleText.text = Customer.Order.Drink.Name;
            _orderImage.sprite = Customer.Order.Drink.DrinkSprite;
            //Name each side ingredient on a new line. Clear first to wipe inspector written scene view test content.
            _orderSideIngredientText.text = "";
            foreach(SideIngredientData si in Customer.Order.SideIngredients)
            {
                _orderSideIngredientText.text += si.Name + "\n";
            }
        }
    }
}
