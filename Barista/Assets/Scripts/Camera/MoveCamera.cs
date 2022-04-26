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
        private bool transitioning;

        [SerializeField]
        private float speed;

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
            if (Input.GetKeyDown(KeyCode.RightArrow) && !transitioning)
            {
                Debug.Log("Right arrow presed");
                StartCoroutine(SwitchPos(true));
            }
            //Right position moving to left
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && !transitioning)
            {
                Debug.Log("Left arrow presed");
                StartCoroutine(SwitchPos(false));
            }
        }

        //lerps the camera position to position A or B depending on current location.
        private IEnumerator SwitchPos(bool isPosA)
        {
            Debug.Log("SwitchPos started");
            float time = 0;
            float duration = 1;
            transitioning = true;

            while (time < duration)
            {
                //lerps left to right
                if(isPosA)
                {
                    transform.position = Vector3.Lerp(posA, posB, camMove.Evaluate(time));
                }
                //lerps right to left
                else
                {
                    transform.position = Vector3.Lerp(posB, posA, camMove.Evaluate(time));
                }
                time += Time.deltaTime * speed;
                yield return null;
            }
            transitioning = false;
        }
    }
}
