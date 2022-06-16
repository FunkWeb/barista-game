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


        public void OnActivation()
        {
            var recipeWindow = Instantiate(_recipeWindowPrefab, Camera.main.WorldToScreenPoint(Vector3.zero), Quaternion.identity, _canvas.transform);
        }
    }
}
