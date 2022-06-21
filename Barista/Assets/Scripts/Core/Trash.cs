using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class Trash : MonoBehaviour, IClickable
    {
        [SerializeField]
        private Drink _drink;

        [SerializeField]
        private Sprite _hoverSprite;
        [SerializeField]
        private Sprite _clickedSprite;

        private void OnAwake()
        {
            if (_drink == null)
                _drink = FindObjectOfType<Drink>();
        }

        public void OnActivation()
        {
            _drink.Clear();
        }
        //Provide sprites for this object's clickable component states.
        public Sprite GetHoverSprite()
        {
            return _hoverSprite;
        }
        public Sprite GetClickedSprite()
        {
            return _clickedSprite;
        }
    }
}
