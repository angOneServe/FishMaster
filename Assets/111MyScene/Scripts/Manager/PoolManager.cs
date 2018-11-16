using GameAttribute;
using GameData;
using GameEffect;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    [Serializable]
    public class PoolManager : ScriptableObject
    {
  
        //鱼
        public FishData[] fishDataArray;
        //子弹
        public BulletData[] bulletDataArray;
        //网
        public WebData[] webDataArray;
        //池

        public GameObjectPool fishPool;
        public GameObjectPool bulletPool;
        public GameObjectPool webPool;
        public GameObjectPool bigGoldPool;
        public GameObjectPool littelGoldPool;

        //初始化
        public void Init()
        {
            fishPool.Init();
            bulletPool.Init();
            webPool.Init();
            bigGoldPool.Init();
            littelGoldPool.Init();
        }
        public bool InitialComplete
        {
            get
            {
                if (fishPool.InitialComplete
                    && bulletPool.InitialComplete
                    && webPool.InitialComplete
                    && bigGoldPool.InitialComplete
                    && littelGoldPool.InitialComplete == true)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
