using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Funksoft.Barista
{
    public class RegisterDisplay : MonoBehaviour
    {
        private DayManager _dayManager;
        private Canvas _canvas;

        [SerializeField]
        private TextMeshPro _text;

        // Start is called before the first frame update
        void Awake()
        {
            _dayManager = FindObjectOfType<DayManager>();
            _canvas = FindObjectOfType<Canvas>();
        }

        // Update is called once per frame
        void Update()
        {
            _text.text = _dayManager.ShiftTimer.ToString("F0");
        }
    }
}
