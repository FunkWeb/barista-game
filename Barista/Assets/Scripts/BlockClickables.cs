using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valley;

namespace Funksoft.Barista
{
    public class BlockClickables : MonoBehaviourSingleton<BlockClickables>
    {
        public bool BlockEnabled{get; set;} = false;
        
        protected override void Awake()
        {
            base.Awake();
        }
    }
}
