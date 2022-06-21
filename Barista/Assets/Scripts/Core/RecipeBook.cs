using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class RecipeBook : MonoBehaviour, IClickable
    {
        [SerializeField]
        private GameObject _recipeWindowPrefab;
        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private Sprite _hoverSprite;
        [SerializeField]
        private Sprite _clickedSprite;


        public void OnActivation()
        {
            var recipeWindow = Instantiate(_recipeWindowPrefab, Camera.main.WorldToScreenPoint(Vector3.zero), Quaternion.identity, _canvas.transform);
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
