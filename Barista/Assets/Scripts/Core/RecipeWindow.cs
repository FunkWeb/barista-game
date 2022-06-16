using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class RecipeWindow : MonoBehaviour
    {
        [SerializeField]
        private int _pageIndex = 0;

        [SerializeField]
        private int _totalPages = 1;


        public void NextPage()
        {
            //Wrap around to first index if we are brought beyond the page count
            if (_pageIndex >= _totalPages-1)
            {
                _pageIndex = 0;
                return;
            }
            _pageIndex = 0;
        }
        public void PrevPage()
        {
            //Wrap around to last index, if we are brought below the first index
            if (_pageIndex == 0)
            {
                _pageIndex = _totalPages-1;
                return;
            }
            _pageIndex--;
        }

        public void Open()
        {
            enabled = true;
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}
