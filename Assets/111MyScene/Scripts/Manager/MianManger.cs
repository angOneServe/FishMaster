using GameData;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Manager
{

    public class MianManger : MonoBehaviour
    {
        private static MianManger Instance_;
        public static MianManger Instance
        {
            get
            {
                return Instance_;
            }
        }
        public UpdateManager mUpdateManager;
        public HotFixManager mHotFixManager;
        public MainPoolManager mMainPoolManager;
        public GameManager mGameManager;
        public UIManager mUIManager;
        public PlayerManager mPlayerManager;
        public SoundManager mSoundManager;
        private bool HasInitial = false;
        public bool reInitial = false;  //管理器初始化
                                        // Use this for initialization

        //private bool asyncInstan = false;//异步实例化
        //private bool lockSign = false;
        //private GameObject pre;
        //public GameObject[]

        private void Awake()
        {
            Instance_ = this;
        }
        void Start()
        {
             
            //检测更新
            //mUpdateManager.UpdateAndDownload();

            //搭建更新的脚本 的运行环境
            //mHotFixManager.Initial();
            //对象池初始化
            MainPoolManager.Instance.Init();
            //进入游戏


            //Model初始化 引用型数据
            DataModel.Instance.Intial();

            //管理器初始化
            mUIManager.MngInitial();
            mSoundManager.MngInitial();
            mGameManager.MngInitial();
            mPlayerManager.MngInitial();
            HasInitial = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (HasInitial == false) return;
            if (MainPoolManager.Instance == null) return;
            if (MainPoolManager.Instance.InitialComplete == false) return;

            if (reInitial == true)
            {
                mUIManager.MngReInitial();
                mSoundManager.MngReInitial();
                mGameManager.MngReInitial();
                mPlayerManager.MngReInitial();
                reInitial = false;
            }
            mUIManager.MngUpdate();
            mSoundManager.MngUpdate();
            mGameManager.MngUpdate();
            mPlayerManager.MngUpdate();

            //if (asyncInstan = true&&lockSign==false)
            //{
            //    asyncInstan = false;

            //    lockSign == false;//解锁
            //}
        }

        //PlayerManager
        public void GunUpDown(bool isUp)
        {
            mPlayerManager.GunUpDown(isUp);
        }
        public void OnMuteChange(bool isOn)
        {
            mSoundManager.SetMute(isOn);
        }
        //协程创建物体
        public  void AsyncInstantial(GameObject pre,Transform parent,int count,float spaceTime,List<GameObject>list, GameObjectPool gameObjectPool)
        {
            StartCoroutine(InstantiateCoroutine(pre, parent,count, spaceTime, list,gameObjectPool));
        }
        IEnumerator InstantiateCoroutine(GameObject pre,Transform parent, int count, float spaceTime, List<GameObject> list, GameObjectPool gameObjectPool)
        {
            list.Clear();
            for (int i = 0; i < count; i++)
            {
                GameObject go=Instantiate(pre);
                go.SetActive(false);
                list.Add(go);
                go.transform.SetParent(parent);
                yield return new WaitForSeconds(spaceTime);
            }
            gameObjectPool.InitialComplete = true;
        }
    }

}
