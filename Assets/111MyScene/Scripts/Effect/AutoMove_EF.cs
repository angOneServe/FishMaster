using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameEffect
{
    public class AutoMove_EF : MonoBehaviour
    {
        public float speed;
        public Vector3 dir = Vector3.right;
        public Space moveSpace = Space.Self;
        private void Update()
        {
            transform.Translate(dir * Time.deltaTime * speed, moveSpace);
        }

    }
}

