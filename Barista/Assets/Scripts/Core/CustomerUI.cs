using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace Funksoft.Barista
{
    public class CustomerUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private TextMeshProUGUI _timerText;

        [SerializeField]
        private bool _showExtendedInfo = false;

        public Customer Customer{ get; set; }

        // Update is called once per frame
        private void LateUpdate()
        {
            _timerText.text = Customer.TimeRemaining.ToString("F0");
        }

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            _showExtendedInfo = true;
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            _showExtendedInfo = false;
        }
    }
}
