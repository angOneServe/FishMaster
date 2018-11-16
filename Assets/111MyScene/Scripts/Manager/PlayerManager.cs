using GameAttribute;
using GameEffect;
using Manager;
using Model;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;

namespace Manager
{
    public class PlayerManager : BaseManager
    {

        private Camera mainCamera;
        private GunAttribute[] guns;        //所有gun属性
        public GameObject[] bulletPre;      //所有bullet pre
        private Transform bulletHolder;     //放zbullet和web的地方
        private int bullectIndex = 0;         //当前子弹的索引值
        private int currentGunIndex = 0;    //当前枪索引值
        private int GunToBullet = 5;        //一种枪有几种子弹
        public override void MngInitial()
        {
            //获取组件camera 及gun的列表
            bulletHolder = GameObject.Find("BulletHolder").transform;
            mainCamera = GameObject.Find("UICameraDepth1").GetComponent<Camera>();
            int guncount = gameObject.transform.childCount;
            guns = new GunAttribute[guncount];
            for (int i = 0; i < guncount; i++)
            {
                guns[i] = gameObject.transform.GetChild(i).GetComponent<GunAttribute>();
            }
            
        }

        public override void MngUpdate()
        {
            GunMove();
            GunFire();
        }
        //gun的移动
        private void GunMove()
        {
            //得到鼠标在世界坐标系的位置（z==0）
            Vector3 temp = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //得到gun在世界坐标系的位置（使 gunpos.z = temp.z）
            Vector3 gunpos = guns[currentGunIndex].transform.position;
            
            gunpos.z = temp.z;
            print("枪的位置："+gunpos+"  鼠标位置："+temp);
            float angle = Vector3.Angle(Vector3.up, temp - gunpos);
            if (temp.x < gunpos.x)
            {
                angle = -angle;
            }
            guns[currentGunIndex].transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
        }
        //gun开火
        private void GunFire()
        {
            //点在ui上返回
            if (EventSystem.current.IsPointerOverGameObject() == true) return;

            if (Input.GetMouseButtonDown(0))
            {
                //扣除金币 金币不够返回
                if (DataModel.Instance.PayForBullet() == false) return;
                //播发开火音效
                SoundManager.Instance.PlayAudio(SoundManager.FIRE);
                //产生后坐力
                StartCoroutine(FireAnimation());
                //发射子弹
                CreatBullet();
            }
        }
        //后坐力
        IEnumerator FireAnimation()
        {
            Transform gunBody = guns[currentGunIndex].gunBody;
            Vector3 startPos = Vector3.zero;
            Vector3 endPos = gunBody.localPosition + new Vector3(0,-0.1f,0);
            print(startPos + "    "+endPos);
            //Transform target1 = gunAtr.firePosTran;
            
            float timer = 0;
            while (timer < 0.1f)
            {

                gunBody.localPosition = Vector3.MoveTowards(gunBody.localPosition, endPos,0.1f);
                timer += Time.deltaTime;
                yield return null;
            }
           
            while (timer < 0.15f)
            {
                gunBody.localPosition = Vector3.MoveTowards(gunBody.localPosition, startPos, 0.05f);
                timer += Time.deltaTime;
                yield return null;
            }
            gunBody.localPosition = startPos;
        }
        //生成子弹
        private void CreatBullet()
        {

            GunAttribute gunAtr = guns[currentGunIndex];
            //GameObject bulletGo = Instantiate(bulletPre[bullectIndex], gunAtr.firePosTran.position, gunAtr.firePosTran.rotation);
            GameObject bulletGo = MainPoolManager.Instance.GetGameObject(PoolType.BULLET, bullectIndex);
            bulletGo.transform.position = gunAtr.firePosTran.position;
            bulletGo.transform.rotation = gunAtr.firePosTran.rotation;
            BullectArrtibute bullectAtr = bulletGo.GetComponent<BullectArrtibute>();

            //移动效果
            AutoMove_EF moveef = bulletGo.GetComponent<AutoMove_EF>();
            if (moveef == null)
            {
                moveef = bulletGo.AddComponent<AutoMove_EF>();
            }
            moveef.dir = Vector3.up;
            moveef.speed = bullectAtr.moveSpeed;
            //网的效果
            AddWeb_EF webef = bulletGo.GetComponent<AddWeb_EF>();
            if (webef == null)
            {
                webef = bulletGo.AddComponent<AddWeb_EF>();
            }
            webef.gunIndex = currentGunIndex;
            webef.lvInWeb = gunAtr.lvInGun;   //获取这种类型枪威力的等级
            webef.webTotaLv = GunToBullet;
            webef.webWaitTime = bullectAtr.webWaitTime; //设置网的停留时间
            webef.damage = bullectAtr.damage;
            webef.SetColor();
            //碰撞鱼销毁效果
            AutoTriggerDestroy_EF trigEF = bulletGo.GetComponent<AutoTriggerDestroy_EF>();
            if (trigEF == null)
            {
                trigEF = bulletGo.AddComponent<AutoTriggerDestroy_EF>();
                trigEF.checkTag = new string[] { "Fish", "Wall" };
            }
           
        }
        //gun升降级
        public void GunUpDown(bool isUp)
        {
            if (isUp)
            {
                //得到等级减一后的子弹与枪
                // bullectIndex--;//会成复数，数组越界
                bullectIndex += GunToBullet * guns.Length - 1;
            }
            if (!isUp)
            {
                //得到等级+1后的子弹与枪
                bullectIndex++;
            }
            SoundManager.Instance.PlayAudio(SoundManager.CHANGE_GUN);
            bullectIndex = bullectIndex % (GunToBullet * guns.Length);
            guns[currentGunIndex].gameObject.SetActive(false);
            currentGunIndex = bullectIndex / GunToBullet;
            guns[currentGunIndex].gameObject.SetActive(true);
            //换gun

            //设置同种枪，枪内等级
            guns[currentGunIndex].lvInGun = bullectIndex % GunToBullet;
            //将等级同步到DataModel
            DataModel.Instance.bulletLv = bullectIndex;
        }


    }

}
