using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Manager
{
    public abstract class BaseManager : MonoBehaviour
    {
        public abstract void MngInitial();
        public virtual void MngReInitial()
        { }
        public virtual void MngUpdate()
        { }
    }
}


