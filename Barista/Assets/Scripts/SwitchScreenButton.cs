using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;

namespace Funksoft.Barista
{
    public class SwitchScreenButton : MonoBehaviour, IEventReceiver<MoveCamera.CamMoveStarted>, IEventReceiver<MoveCamera.CamMoveFinished>
    {
        private Vector3 _initialPos;
        private Vector3 _oppositePos;

        private RectTransform rectTransform;

        private void Awake()
        {
            TryGetComponent<RectTransform>(out rectTransform);
        }

        void Start()
        {

        }

        private void OnEnable()
        {
            EventBus.Register(this);
        }
        private void OnDisable()
        {
            EventBus.UnRegister(this);
        }

        public void OnEvent(MoveCamera.CamMoveStarted e)
        {
            Debug.Log("Test");
            rectTransform.localScale = new Vector3(0, 0, 0);
        }
        public void OnEvent(MoveCamera.CamMoveFinished e)
        {
            rectTransform.localScale = new Vector3(1, 1, 1);

            if (e.screenMovedTo == MoveCamera.Screen.LeftScreen)
            {
                rectTransform.anchorMin = new Vector2(1f, rectTransform.anchorMin.y);
                rectTransform.anchorMax = new Vector2(1f, rectTransform.anchorMax.y);
            }
            else if (e.screenMovedTo == MoveCamera.Screen.RightScreen)
            {
                rectTransform.anchorMin = new Vector2(0f, rectTransform.anchorMin.y);
                rectTransform.anchorMax = new Vector2(0f, rectTransform.anchorMax.y);
            }

            transform.position = new Vector3(0, 0, 0);
        }
    }
}
