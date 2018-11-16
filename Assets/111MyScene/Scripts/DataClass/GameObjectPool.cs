using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Collections;
using Manager;

namespace GameData
{
    [Serializable]
    public class GameObjectPool 
    {
        [SerializeField]
        private string parentName;
        [SerializeField]
        private  GameObject prefab;
        [SerializeField]
        private int maxCount;
        private List<GameObject> GoList=new List<GameObject>();
        [SerializeField]
        private float timeSpace;             //创建的时间间隔
        private Transform parent;
        [NonSerialized]
        private bool initialComplete=false;     //是否创建完成
        public bool InitialComplete
        {
            get
            {
                return initialComplete;
            }
            set
            {
                initialComplete = value;
            }
        }
        //初始化
        public void Init()
        {

            //安全检验
            if (prefab == null) return;
            if (maxCount <= 0) return;
            if (timeSpace <= 0) return;
            if (parentName == null) return;

            GameObject go = new GameObject(parentName);
            parent = go.transform;
            //启动协程创建池
            MianManger.Instance.AsyncInstantial(prefab, parent,maxCount, timeSpace, GoList, this);
      
        }
        

        //得到池中对象
        public GameObject GetGameObject()
        {
            for (int i = 0; i < GoList.Count; i++)
            {
                
                if (GoList[i].activeSelf == false)
                {
                    //TODO 对索引值控制的优化 大于1/3比例索引倒着取
                    GoList[i].SetActive(true);
                    return GoList[i];
                }
            }
            return GameObject.Instantiate(prefab);
        }

        //尝试放回对象 失败返回false
        public bool DisEnablePoolGo(GameObject go)
        {
            if (GoList.Contains(go) == false) return false;
            go.SetActive(false);
            return true;
        }
    }
}

