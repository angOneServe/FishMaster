using GameEffect;
using GameTools;
using Manager;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAttribute
{
    public class FishAttribute : MonoBehaviour
    {

        public int maxNum;          //每批次生成的最大数量
        public float moveSpeed;     //移动的最大速度
        public int gold;            //可以换的金币
        public int exp;             //可得经验值
        public int hp;              //鱼的血量
        public GameObject goldPre;  //金币预制体
        public GameObject littleGoldPre;
        private Animator animator;  //鱼的动画控制器
        private void Start()
        {
            animator = transform.GetComponent<Animator>();
        }
        //受到渔网的伤害
        public void BeHurt(int damage)
        {
            hp -= damage;
            if (hp <= 0)
            {
                //TODO 死亡动画 音效
                animator.SetTrigger("die");
                gameObject.GetComponent<AutoMove_EF>().enabled = false;
                //PlayCatchAudio();
                Invoke("DestroySelf", 0.3f);
                //
                HandleFishGoldeExp();
                //生成金币 
                Invoke("CreatGold", 0.26f);
            }
        }
        //播放抓捕声效
        private void PlayCatchAudio()
        {
            string audioname;
            switch (gameObject.name)
            {
                case "枪鱼(Clone)":
                    audioname = Random.Range(0, 1f) > 0.5f ? SoundManager.FISH_JINQIANG1 : SoundManager.FISH_JINQIANG2;
                    break;
                case "灯笼鱼(Clone)":
                    audioname = Random.Range(0, 1f) > 0.5f ? SoundManager.FISH_DENGLONG1 : SoundManager.FISH_DENGLONG2;
                    break;
                case "蝴蝶鱼(Clone)":
                    audioname = Random.Range(0, 1f) > 0.5f ? SoundManager.FISH_BIANFU1 : SoundManager.FISH_BIANFU2;
                    break;
                case "金鲨(Clone)":
                    audioname = Random.Range(0, 1f) > 0.5f ? SoundManager.FISH_JINSHA1 : SoundManager.FISH_JINSHA2;
                    break;
                case "银鲨(Clone)":
                    audioname = Random.Range(0, 1f) > 0.5f ? SoundManager.FISH_YINSHA1 : SoundManager.FISH_YINSHA2;
                    break;
                case "魔鬼鱼(Clone)":
                    audioname = Random.Range(0, 1f) > 0.5f ? SoundManager.FISH_LANSHA1 : SoundManager.FISH_LANSHA2;
                    break;
                default:
                    audioname = Random.Range(0, 1f) > 0.5f ? SoundManager.FISH_WUGUI1 : SoundManager.FISH_WUGUI2;
                    break;

            }
            MianManger.Instance.mSoundManager.PlayAudio(audioname);
        }
        //生成金币
        private void CreatGold()
        {
            // int BigGoldNum = gold / 30;
            //int littleGoldNum = (gold % 30) / 5;
            // for (int i = 0; i < BigGoldNum; i++)
            // {
            // Instantiate(goldPre,transform.position,transform.rotation,DataModel.Instance.GoldCollectBox);
            //}
            //for (int i = 0; i < littleGoldNum; i++)
            // {
            //  Instantiate(littleGoldPre, transform.position, transform.rotation, DataModel.Instance.GoldCollectBox);
            //  }
            if (gold > 100)
            {
                //GameObject goldGo = Instantiate(goldPre, transform.position, transform.rotation, DataModel.Instance.GoldCollectBox);
                GameObject goldGo = MainPoolManager.Instance.GetGameObject(PoolType.BIGGOLD, 0);
                goldGo.transform.position = transform.position;
                goldGo.transform.rotation = transform.rotation;
                goldGo.name = "Gold";
            }
            else
            {
                //GameObject goldGo = Instantiate(littleGoldPre, transform.position, transform.rotation, DataModel.Instance.GoldCollectBox);
                GameObject goldGo = MainPoolManager.Instance.GetGameObject(PoolType.LITTLEGOOLD, 0);
                goldGo.transform.position = transform.position;
                goldGo.transform.rotation = transform.rotation;
                goldGo.name = "littleGold";
            }
        }
        //处理鱼的gold exp 
        private void HandleFishGoldeExp()
        {
            DataModel.Instance.AddGoldExp(gold, exp);
        }
        //处理销毁或设置activeself
        private void DestroySelf()
        {
            Tools.MyDestroy(this.gameObject);
        }
    }

}