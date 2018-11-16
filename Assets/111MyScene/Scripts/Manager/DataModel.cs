using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
//using XLua;
namespace Model
{
   // [Hotfix]
    public class DataModel
    {
        private static DataModel Instance_;
        public static DataModel Instance
        {
            get
            {
                if (Instance_ == null)
                {
                    Instance_ = new DataModel();
                }
                return Instance_;
            }
        }
        private DataModel() { }

        public Transform GoldCollectBox;
        public bool uiDataNeedUpdata = false;
        //数据
        public int lv;                  //等级
        public string[] lvNameArray = { "新手", "青木", "青铜", "黑铁", "白银", "黄金", "铂金", "钻石", "荣耀", "王者" };   //等级称号
                                                                                                        //public string lvName;

        public int lvExp;               //升级需要的经验值
        public int currentExp;          //当前经验值
        public int gold;                //金币数额
        private int goldInitialValue = 500000; //金币初始值

        public const int bigCountDown = 10; //240   //大计时器倒计时初值 
        public int bigCountDownGold = 50;       //计时结束奖励的金币数
        public const int smallCountDown = 60;  //60  //小倒计时计时初值


        public int[] bulletPriceArray = new int[] { 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400, 1500, 1600 };                    //枪的等级当前花费的金币
        public int bulletLv = 0;                       //枪的等级
                                                       //asset资源
        public Sprite[] bgSprites;
        private int currentBgIndex = 0;

        public void Intial()
        {
            LoadAsset();
            InitialGameData();

        }
        //加载游戏资源
        private void LoadAsset()
        {
            GoldCollectBox = GameObject.Find("GoldCollectBox").transform;
            bgSprites = Resources.LoadAll<Sprite>("BGImage");
        }
        //初始化游戏数据
        public void InitialGameData()
        {
            lv = 0;
            lvExp = GetLvExp();
            currentExp = 0;
            gold = goldInitialValue;
            bulletLv = 0;
        }

        ////换算数据

        //得到升级需要的经验
        public int GetLvExp()
        {
            float temp = 2000 + 10 * Mathf.Pow(lv, 4) + 5 * Mathf.Pow(lv, 3);
            return (int)temp;
        }
        //得到下一个背景图片
        public Sprite ExchangeBG()
        {
            currentBgIndex = (currentBgIndex + 1) % bgSprites.Length;
            return bgSprites[currentBgIndex];
        }
        //得到经验值比值
        public float GetExpSliderValue()
        {
            //根据经验升级
            while (currentExp >= lvExp)
            {
                currentExp -= lvExp;
                lv++;
                lvExp = GetLvExp();
            }
            return currentExp * 1.0f / lvExp;
        }
        //得到称号
        public string GetLvName()
        {
            return lvNameArray[Mathf.Min(lv, lvNameArray.Length - 1)];
        }
        //得到枪等级对应的金币价格
        public int GetBullectPrice()
        {

            if (bulletLv < 0) bulletLv = 0;
            if (bulletLv >= bulletPriceArray.Length) bulletLv = bulletPriceArray.Length - 1;
            return bulletPriceArray[bulletLv];
        }
        //尝试为子弹支付金币，失败返回false
        public bool PayForBullet()
        {
            //提示ui更新标志位
            uiDataNeedUpdata = true;

            if (gold < bulletPriceArray[bulletLv])
            {
                return false;
            }
            else
            {
                gold -= bulletPriceArray[bulletLv];
                return true;
            }
        }
        //捕鱼后加金币与经验
        public void AddGoldExp(int gold, int exp)
        {
            uiDataNeedUpdata = true;
            this.gold += gold;
            this.currentExp += exp;

        }

        public void RespondUiUpdate()
        {
            uiDataNeedUpdata = false;
        }
    }

}

