using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameData
{
    [Serializable]
    public class FishData
    {
        public string name;
        public Sprite sprite;
        public RuntimeAnimatorController controller;
        public Vector2 EdgePadius;
        public int maxNum;
        public float moveSpeed;
        public int gold;
        public int exp;
        public int hp;
        public GameObject goldPre;
        public GameObject littleGoldPre;

    }
}

