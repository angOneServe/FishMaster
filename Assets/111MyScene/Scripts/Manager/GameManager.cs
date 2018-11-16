using GameAttribute;
using GameData;
using GameEffect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class GameManager : BaseManager
    {
        public Transform[] FishCreatPoint;
        public GameObject[] FishPre;
        private int BgOrder = 0;

        //生成器的属性
        private float CreatStreamTime = 0.4f; //多少时间生成一批
        private float CreatFishTime = 0.3f;   //一批的生成间隔
        private float CreatStreamTimer = 0;
        private float isBigFish = 0.3f;     //每波生成大鱼的可能性

        public override void MngInitial()
        {

        }
        public override void MngUpdate()
        {
            //生成批次的控制
            CreatStreamTimer += Time.deltaTime;
            if (CreatStreamTimer < CreatStreamTime) return;
            CreatStreamTimer = 0;

            //得到产鱼位置 index
            int posIndex = Random.Range(0, FishCreatPoint.Length / 2);
            //得到产鱼种类 index 
            int fishIndex;
            if (Random.Range(0f, 1f) < isBigFish)
            {
                fishIndex = Random.Range(0, FishPre.Length);
            }
            else
            {
                fishIndex = Random.Range(FishPre.Length / 4+1, FishPre.Length);
            }
            //得到该种鱼的一些属性
            FishData fishData = MainPoolManager.Instance.GetData(PoolType.FISH, fishIndex) as FishData;
            //此批次产鱼数
            int creatNum = Random.Range(fishData.maxNum / 2 + 1, fishData.maxNum);
            //鱼的速度  (1-fishIndex/FishPre.Length/3f)位置靠后的鱼速度越慢
            float speed = Random.Range(fishData.moveSpeed / 2f, fishData.moveSpeed) * (1 - fishIndex / FishPre.Length / 3f);
            Transform fishCreatPos = FishCreatPoint[posIndex];
            //得到鱼游的属性
            float line = Random.Range(0f, 1f);
            if (line < 0.5f)//直线
            {
                float straightAngle = Random.Range(-22f, 22f);

                StartCoroutine(CreatStraightFishStram(fishCreatPos, fishIndex, creatNum, speed, straightAngle));

            }
            else//曲线
            {
                float rotateAngle;
                float rotateAngleSign = Random.Range(0f, 1f);
                if (rotateAngleSign < 0.5f)
                {
                    rotateAngle = Random.Range(-8f, -15f);
                }
                else
                {
                    rotateAngle = Random.Range(8f, 15f);
                }

                StartCoroutine(CreatTrunFishStram(fishCreatPos, fishIndex, creatNum, speed, rotateAngle));
            }



        }
        //让鱼游起来
        IEnumerator CreatStraightFishStram(Transform fishCreatPos, int fishIndex, int creatNum, float speed, float straightAngle)
        {
            for (int i = 0; i < creatNum; i++)
            {
                //生成鱼
                GameObject go = MainPoolManager.Instance.GetGameObject(PoolType.FISH, fishIndex);
                go.transform.position = fishCreatPos.position;
                go.transform.rotation = fishCreatPos.rotation;
                //设置倾斜
                go.transform.Rotate(new Vector3(0, 0, straightAngle));
                //设置order
                go.GetComponent<SpriteRenderer>().sortingOrder = BgOrder + fishIndex * 10 + i;

                //挂载属性脚本(没有就挂载且设置数据，否则只设置数据)
                AutoMove_EF moveef = go.GetComponent<AutoMove_EF>();
                if (go.GetComponent<AutoMove_EF>() == null)
                {
                    moveef = go.AddComponent<AutoMove_EF>();
                    AutoTriggerDestroy_EF colDesEf = go.AddComponent<AutoTriggerDestroy_EF>();
                    colDesEf.checkTag = new string[] { "Wall" };
                }
                if (moveef.enabled == false)
                {
                    moveef.enabled = true;
                }
                moveef.speed = speed;

                yield return new WaitForSeconds(CreatFishTime);
            }
        }
        IEnumerator CreatTrunFishStram(Transform fishCreatPos, int fishIndex, int creatNum, float speed, float rotateAngle)
        {
            for (int i = 0; i < creatNum; i++)
            {
                //生成鱼
                GameObject go = MainPoolManager.Instance.GetGameObject(PoolType.FISH, fishIndex);
                go.transform.position = fishCreatPos.position;
                go.transform.rotation = fishCreatPos.rotation;
                //设置order
                go.GetComponent<SpriteRenderer>().sortingOrder = BgOrder + fishIndex * 10 + i;
                //挂载属性脚本

                AutoMove_EF moveef = go.GetComponent<AutoMove_EF>();
                AutoRotate_EF rotateef = go.GetComponent<AutoRotate_EF>();
                if(moveef==null)
                { 
                    moveef = go.AddComponent<AutoMove_EF>();
                    AutoTriggerDestroy_EF colDesEf = go.AddComponent<AutoTriggerDestroy_EF>();
                    colDesEf.checkTag = new string[] { "Wall" };
                }
                if (rotateef == null)
                {
                    rotateef = go.AddComponent<AutoRotate_EF>();
                }
                if (moveef.enabled == false)
                {
                    moveef.enabled = true;
                }
                moveef.speed = speed;
                rotateef.angleV = rotateAngle;
                
                yield return new WaitForSeconds(CreatFishTime);
            }
        }

    }

}