using GameTools;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameEffect
{
    public class AutoDestroy_EF : MonoBehaviour
    {
        public float time = 1f;
        public void OnEnable()
        {
            Invoke("DestroySelf", time);
        }

        private void DestroySelf()
        {
            Tools.MyDestroy(this.gameObject);
        }
    }
}

