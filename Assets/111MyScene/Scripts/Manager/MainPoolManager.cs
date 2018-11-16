using GameAttribute;
using GameData;
using GameEffect;
using Manager;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace Manager
{

    //池的类型
    public enum PoolType
    {
        FISH,
        BULLET,
        WEB,
        BIGGOLD,
        LITTLEGOOLD
    }
    public class MainPoolManager : MonoBehaviour
    {
        PoolManager poolManager;
        //单例
        private static MainPoolManager Instance_;
        public static MainPoolManager Instance
        {
            get
            {
                return Instance_;
            }
        }
        public bool InitialComplete
        {
            get
            {
                if (poolManager.InitialComplete == true)
                {
                    return true;
                }
                return false;
            }
        }
        //初始化
        public void Awake()
        {
            Instance_ = this;
        }
        public void Init()
        {
            poolManager = Resources.Load<PoolManager>("poolManager");
            if (poolManager == null)
            {
                Debug.LogWarning("PoolManager的配置文件不存在，请点击ManagerTools/CreatPoolList进行生成");
                return;
            }
            poolManager.Init();
        }
        //获取
        public GameObject GetGameObject(PoolType poolType, int dataIndex)
        {
            GameObject go;
            switch (poolType)
            {
                case PoolType.FISH:
                    go = poolManager.fishPool.GetGameObject();
                    FishData fishData = poolManager.fishDataArray[dataIndex];
                    return AddFishData(go, fishData);
                    break;
                case PoolType.BULLET:
                    go = poolManager.bulletPool.GetGameObject();
                    BulletData bulletData = poolManager.bulletDataArray[dataIndex];
                    return AddBulletData(go, bulletData);
                    break;
                case PoolType.WEB:
                    go = poolManager.webPool.GetGameObject();
                    WebData webData = poolManager.webDataArray[dataIndex];
                    return AddWebData(go, webData);
                    break;
                case PoolType.BIGGOLD:
                    return poolManager.bigGoldPool.GetGameObject();
                    break;
                case PoolType.LITTLEGOOLD:
                    return poolManager.littelGoldPool.GetGameObject();
                    break;
                default: break;
            }
            return null;
        }


        //鱼 信息添加
        private GameObject AddFishData(GameObject fish, FishData fishData)
        {
            fish.GetComponent<Animator>().runtimeAnimatorController = fishData.controller;
            FishAttribute fa = fish.GetComponent<FishAttribute>();
            fa.maxNum = fishData.maxNum;
            fa.moveSpeed = fishData.moveSpeed;
            fa.gold = fishData.gold;
            fa.exp = fishData.exp;
            fa.hp = fishData.hp;
            //fa.goldPre = fishData.goldPre;
            //fa.littleGoldPre = fishData.littleGoldPre;
            fish.GetComponent<BoxCollider2D>().size = fishData.EdgePadius;
            fish.name = fishData.name;
            return fish;
        }
        //子弹信息添加
        private GameObject AddBulletData(GameObject bullet, BulletData bulletData)
        {
            bullet.GetComponent<AutoAnimation_ef>().sprites = bulletData.AutpAnimationElements;
            BullectArrtibute bulletAtr = bullet.GetComponent<BullectArrtibute>();
            bulletAtr.moveSpeed = bulletData.moveSpeed;
            bulletAtr.webWaitTime = bulletData.webWaitTime;
            bulletAtr.damage = bulletData.damage;
            bullet.GetComponent<BoxCollider2D>().size = bulletData.EdgeRadius;
            bullet.GetComponent<SpriteRenderer>().sprite = bulletData.sprite;
            bullet.name = bulletData.name;


            return bullet;
        }
        //渔网信息添加
        private GameObject AddWebData(GameObject web, WebData webData)
        {
            web.GetComponent<SpriteRenderer>().sprite = webData.sprite;
            web.GetComponent<CircleCollider2D>().radius = webData.Radius;
            web.transform.localScale = new Vector3(webData.scale, webData.scale, 1);
            return web;
        }
        //金币信息添加

        //得到信息(只是对应poolType，dataIndex信息)
        public object GetData(PoolType poolType, int dataIndex)
        {
            switch (poolType)
            {
                case PoolType.FISH:
                    return poolManager.fishDataArray[dataIndex];
                case PoolType.BULLET:
                    return poolManager.bulletDataArray[dataIndex];
                case PoolType.WEB:
                    return poolManager.webDataArray[dataIndex];
                default:
                    return null;
            }

        }

        public bool DisEnablePoolGo(GameObject go)
        {
            bool sign =
                poolManager.fishPool.DisEnablePoolGo(go) ||
                poolManager.webPool.DisEnablePoolGo(go) ||
                poolManager.bulletPool.DisEnablePoolGo(go) ||
                poolManager.bigGoldPool.DisEnablePoolGo(go) ||
                poolManager.littelGoldPool.DisEnablePoolGo(go);
            if (sign == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
