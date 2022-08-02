using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        void Awake()
        {
            //Hard coding positions because they don't need to be changed once set.
            //Might ned to adjust 19.2 based on background size. As well as camera projections size 5.4.
            _posA = transform.localPosition;
            _posB = new Vector3(transform.localPosition.x + 19.2f, 0, -10);
        }

        void Update()
        {
            //left position moving to right
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
        }
    }
}
