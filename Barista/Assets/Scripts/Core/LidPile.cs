using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;

namespace Funksoft.Barista
{
    public class LidPile : MonoBehaviour, IClickable
    {
        [SerializeField]
        private string _displayName;


        [SerializeField]
        private Sprite _hoverSprite;
        [SerializeField]
        private Sprite _clickedSprite;


        public struct AttemptLidDrink : IEvent{}

        public void OnActivation()
        {
            EventBus<AttemptLidDrink>.Raise(new AttemptLidDrink());
        }

        public string GetDisplayName()
        {
            return _displayName;
        }

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
