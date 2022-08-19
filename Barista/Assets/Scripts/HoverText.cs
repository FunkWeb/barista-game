using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;
using TMPro;

namespace Funksoft.Barista
{
    public class HoverText : MonoBehaviour, IEventReceiver<ClickableObject.HoverEvent>
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            TryGetComponent<TextMeshProUGUI>(out _text);
            _text.text = "";
        }

        private void OnEnable()
        {
            EventBus.Register(this);
        }
        private void OnDisable()
        {
            EventBus.UnRegister(this);
        }

        //Receive event from clickableObject with name for display on mouse hover.
        public void OnEvent(ClickableObject.HoverEvent e)
        {
                _text.text = e.objectDisplayName;
        }
    }
}
