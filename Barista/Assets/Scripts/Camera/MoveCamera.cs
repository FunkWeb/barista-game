using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funksoft.Barista
{
    public class MoveCamera : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve camMove;

        private Vector3 posA;
        private Vector3 posB;
        private bool posLock = true;

        
        private float duration = 1;

        void Awake()
        {
            //Hard coding positions because they don't need to be changed once set.
            //Might ned to adjust 19.2 based on background size. As well as camera projections size 5.4.
            posA = transform.localPosition;
            posB = new Vector3(transform.localPosition.x + 19.2f, 0, -10);
            
        }

        void Update()
        {
            //left position moving to right
            if (Input.GetKeyDown(KeyCode.RightArrow)&& posLock)
            {
                Debug.Log("Right arrow presed");
                StartCoroutine(SwitchPos(true));
                posLock = false;
            }
            //Right position moving to left
            else if (Input.GetKeyDown(KeyCode.LeftArrow)&& !posLock)
            {
                Debug.Log("Left arrow presed");
                StartCoroutine(SwitchPos(false));
                posLock = true;
            }
        }

        //lerps the camera position to position A or B depending on current location.
        private IEnumerator SwitchPos(bool isPosA)
        {
            Debug.Log("SwitchPos started");

            float time = 0;
            float rate = 1/duration;

            while (time < duration)
            {
                if(isPosA)
                {
                    transform.position = Vector3.Lerp(posA, posB, camMove.Evaluate(time));
                }
                else
                {
                    transform.position = Vector3.Lerp(posB, posA, camMove.Evaluate(time));
                }
                time += rate * Time.deltaTime;
                yield return null;
            }
        }
    }
}
