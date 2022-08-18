using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pEventBus;

namespace Funksoft.Barista
{
    public class MoveCamera : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve _camMove;

        private Vector3 _posA;
        private Vector3 _posB;
        private bool _transitioning;
        bool _isStartPos = true;

        [SerializeField]
        private float speed;

        public enum Screen
        {
            LeftScreen,
            RightScreen
        }

        public struct CamMoveFinished : IEvent
        {
            public Screen screenMovedTo;
        }

        public struct CamMoveStarted : IEvent
        {
            public Screen screenMovingTo;
        }

        void Awake()
        {
            //Hard coding positions because they don't need to be changed once set.
            //Might ned to adjust 19.2 based on background size. As well as camera projections size 5.4.
            _posA = transform.localPosition;
            _posB = new Vector3(transform.localPosition.x + 19.2f, 0, -10);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartSwitch();
            }
        }

        private void StartSwitch()
        {
            if (_transitioning)
                return;

            StartCoroutine(SwitchPos(_isStartPos));
            _isStartPos = !_isStartPos;
        }

        //lerps the camera position to position A or B depending on current location.
        private IEnumerator SwitchPos(bool isPosA)
        {
            float time = 0;
            float duration = 1;
            _transitioning = true;

            Screen screenMovingTo;

            if (isPosA)
                screenMovingTo = Screen.RightScreen;
            else
                screenMovingTo = Screen.LeftScreen;

            #region Raise CameraMoveStarted event, and provide which screen camera is moving towards.
            if (screenMovingTo == Screen.LeftScreen)
            {
                EventBus<CamMoveStarted>.Raise
                (
                    new CamMoveStarted
                    {
                        screenMovingTo = Screen.LeftScreen
                    }
                );
            }
            else if (screenMovingTo == Screen.RightScreen)
            {
                EventBus<CamMoveStarted>.Raise
                (
                    new CamMoveStarted
                    {
                        screenMovingTo = Screen.RightScreen
                    }
                );
            }
            #endregion

            #region Do Camera Move over time
            while (time < duration)
            {
                //lerps left to right
                if(isPosA)
                {
                    transform.position = Vector3.Lerp(_posA, _posB, _camMove.Evaluate(time));
                }
                //lerps right to left
                else
                {
                    transform.position = Vector3.Lerp(_posB, _posA, _camMove.Evaluate(time));
                }
                time += Time.deltaTime * speed;
                yield return null;
            }
            _transitioning = false;
            #endregion
            
            //Set transform to exact destination position after transition, and store which screen we arrived at.
            Screen screenArrivedAt;
            if (isPosA)
            {
                transform.position = _posB;
                screenArrivedAt = Screen.RightScreen;
            }
            else
            {
                transform.position = _posA;
                screenArrivedAt = Screen.LeftScreen;
            }

            #region Raise CameraMoveFinished event, and provide which screen camera moved towards
            if (screenArrivedAt == Screen.LeftScreen)
            {
                EventBus<CamMoveFinished>.Raise
                (
                    new CamMoveFinished
                    {
                        screenMovedTo = Screen.LeftScreen
                    }
                );
            }
            else if (screenArrivedAt == Screen.RightScreen)
            {
                EventBus<CamMoveFinished>.Raise
                (
                    new CamMoveFinished
                    {
                        screenMovedTo = Screen.RightScreen
                    }
                );
            }
            #endregion

        }
    }
}
