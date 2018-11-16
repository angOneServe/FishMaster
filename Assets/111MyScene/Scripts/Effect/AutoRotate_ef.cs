using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameEffect
{
    public class AutoRotate_EF : MonoBehaviour
    {
        public Vector3 axis = Vector3.forward;
        public float angleV;    //每秒转多少度
        private void Update()
        {
            transform.Rotate(axis, angleV * Time.deltaTime);
        }
    }
}

