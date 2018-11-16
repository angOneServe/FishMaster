using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameAttribute
{
    public class GunAttribute : MonoBehaviour
    {
        public int needGold;    //需要的金币数量
        public int lvInGun = 0;    //同样的枪不同的等级 0开始
        public GameObject web;  //枪所对应的网
        public Transform firePosTran;   //开火位置
        public Transform gunBody;
        private void Start()
        {
            firePosTran = transform.GetChild(0);
            gunBody = transform.GetChild(1);
        }

    }

}


