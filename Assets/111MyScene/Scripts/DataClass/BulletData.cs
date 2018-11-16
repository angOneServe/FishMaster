using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameData
{
    [Serializable]
    public class BulletData
    {
        public string name;
        public Vector2 EdgeRadius;
        public Sprite sprite;        
        public Sprite[] AutpAnimationElements; 
        public float moveSpeed;
        public float webWaitTime;
        public int damage;
    }

}
